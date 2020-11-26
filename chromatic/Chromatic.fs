namespace Chromatic

[<Struct>]
type Color =
  {
    Red : uint8
    Green : uint8
    Blue : uint8
    Alpha : uint8
  }

[<Struct>]
type HSL =
  {
    Hue : float
    Saturation : float
    Luminosity : float
  }

module Color =

  module internal Int32 =

    open System

    let tryParse (x : string) =
      match Int32.TryParse x with
      | (true, i) -> Some i
      | _ -> None

  module internal Byte =

    open System
    open System.Globalization

    let tryParse (x : string) =
      match Byte.TryParse x with
      | (true, i) -> Some i
      | _ -> None

#if FABLE_COMPILER
    open Fable.Core
    open System

    [<Emit("parseInt($0, 16)")>]
    let private parseInt16JS (x : string) : float = jsNative

    let tryParseHex (x : string) =
      let x = parseInt16JS x

      if Double.IsNaN x
      then
        None
      else
        Some (byte x)
#else
    let tryParseHex (x : string) =
      match Byte.TryParse (x, NumberStyles.HexNumber, null) with
      | (true, i) -> Some i
      | _ -> None
#endif

  let create r g b a =
    {
      Red = r
      Green = g
      Blue = b
      Alpha = a
    }

  let hex (c : Color) =
    let channel (c : uint8) =
      let h = c.ToString("X")
      if h.Length = 1 then "0" + h else h

    let x =
      "#" + channel c.Red
      + channel c.Green
      + channel c.Blue
      + channel c.Alpha

    x.ToLowerInvariant ()

  let tryParseHex (x : string) =
    let x =
      if x.StartsWith "#"
      then
        x.Substring 1
      else
        x

    if x.Length = 6
    then
      let rPart = Byte.tryParseHex (x.Substring (0, 2))
      let gPart = Byte.tryParseHex (x.Substring (2, 2))
      let bPart = Byte.tryParseHex (x.Substring (4, 2))

      match rPart, gPart, bPart with
      | (Some r, Some g, Some b) ->
        Some
          {
            Red = r
            Green = g
            Blue = b
            Alpha = 255uy
          }
      | _ -> None
    elif x.Length = 8
    then
      let rPart = Byte.tryParseHex (x.Substring (0, 2))
      let gPart = Byte.tryParseHex (x.Substring (2, 2))
      let bPart = Byte.tryParseHex (x.Substring (4, 2))
      let aPart = Byte.tryParseHex (x.Substring (6, 2))

      match rPart, gPart, bPart, aPart with
      | (Some r, Some g, Some b, Some a) ->
        Some
          {
            Red = r
            Green = g
            Blue = b
            Alpha = a
          }
      | _ -> None
    else
      None

  let parseHex (x : string) =
    match tryParseHex x with
    | Some x -> x
    | None -> failwithf "\"%s\" is not a valid hex color" x

  // See http://www.w3.org/TR/2008/REC-WCAG20-20081211/#relativeluminancedef
  let luminance (x : Color) =
    let channel x =
      let x = float x / 255.0

      if x <= 0.03928
      then
        x / 12.92
      else
        (((x + 0.055) / 1.055) ** 2.4)

    let r = channel (x.Red)
    let g = channel (x.Green)
    let b = channel (x.Blue)

    0.2126 * r + 0.7152 * g + 0.0722 * b

  let complementary (x : Color) =
    create
      (255uy - x.Red)
      (255uy - x.Green)
      (255uy - x.Blue)
      (x.Alpha)

  let fromTemperature (kelvin : float) =
    let temp = kelvin / 100.0
    let r, g, b =
      if temp < 66.0
      then
        let g = temp - 2.0
        let b = temp - 10.0

        let r = 255.0
        let g = -155.25485562709179 - 0.44596950469579133 * g + 104.49216199393888 * log g
        let b = if temp < 20.0 then 0.0 else -254.76935184120902 + 0.8274096064007395 * b + 115.67994401066147 * log b
        r, g, b
      else
        let r = temp - 55.0
        let g = temp - 50.0

        let r = 351.97690566805693 + 0.114206453784165 * r - 40.25366309332127 * log r
        let g = 325.4494125711974 + 0.07943456536662342 * g - 28.0852963507957 * log g
        let b = 255.0
        r, g, b

    create (byte r) (byte g) (byte b) 255uy

  let temperature (x : Color) =
    let r = float x.Red
    let b = float x.Blue
    let mutable minTemp = 1000.0
    let mutable maxTemp = 40000.0
    let eps = 0.4

    let mutable temp = 0.0

    while (maxTemp - minTemp > eps) do
      temp <- (maxTemp + minTemp) * 0.5

      let rgb = fromTemperature temp

      if float rgb.Blue / float rgb.Red >= b / r
      then
        maxTemp <- temp
      else
        minTemp <- temp

    round temp
