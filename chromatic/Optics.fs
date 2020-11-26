module Chromatic.Optics

// Aether compatible
type Lens<'t, 'u> =
  ('t -> 'u) * ('u -> 't -> 't)

module Color =

  let red : Lens<Color, _> =
    (fun x -> x.Red), (fun v x -> { x with Red = v })

  let green : Lens<Color, _> =
    (fun x -> x.Green), (fun v x -> { x with Green = v })

  let blue : Lens<Color, _> =
    (fun x -> x.Blue), (fun v x -> { x with Blue = v })

  let alpha : Lens<Color, _> =
    (fun x -> x.Alpha), (fun v x -> { x with Alpha = v })

module HSL =

  let hue : Lens<HSL, _> =
    (fun x -> x.Hue), (fun v x -> { x with Hue = v })

  let saturation : Lens<HSL, _> =
    (fun x -> x.Saturation), (fun v x -> { x with Saturation = v })

  let luminosity : Lens<HSL, _> =
    (fun x -> x.Luminosity), (fun v x -> { x with Luminosity = v })
