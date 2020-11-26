open System
open Expecto
open Aether
open Aether.Operators
open Chromatic
open Chromatic.Optics

let tests =
  testList "basic functionality" [

    test "tryParseHex 1" {
      let actual = Color.tryParseHex "#ff0000ff"
      let expected = Some (Color.create 255uy 0uy 0uy 255uy)
      Expect.equal actual expected "The parse result should equal"
    }

    test "tryParseHex 2" {
      let actual = Color.tryParseHex "#c1fe325e"
      let expected = Some (Color.create 193uy 254uy 50uy 94uy)
      Expect.equal actual expected "The parse result should equal"
    }

    test "tryParseHex 3" {
      let actual = Color.tryParseHex "#ff0000"
      let expected = Some (Color.create 255uy 0uy 0uy 255uy)
      Expect.equal actual expected "The parse result should equal"
    }

    test "tryParseHex 4" {
      let actual = Color.tryParseHex "#bcbb8c"
      let expected = Some (Color.create 188uy 187uy 140uy 255uy)
      Expect.equal actual expected "The parse result should equal"
    }

    test "hex" {
      let actual = Color.create 193uy 254uy 50uy 94uy |> Color.hex
      let expected = "#c1fe325e"
      Expect.equal actual expected "The hex code should equal"
    }

    test "luminance" {
      let actual =  Color.luminance Colors.red
      let expected = 0.2126
      Expect.equal actual expected "The luminance should equal"
    }

    test "multiply blend" {
      let actual = Blend.multiply (Color.parseHex "#ff0000") (Color.parseHex "#5a9f37")
      let expected = Color.parseHex "#5a0000"
      Expect.equal actual expected "The blend should equal"
    }

    test "screen blend" {
      let actual = Blend.screen (Color.parseHex "#b83d31") (Color.parseHex "#0da671")
      let expected = Color.parseHex "#bbbb8c"
      Expect.equal actual expected "The blend should equal"
    }

    test "overlay blend" {
      let actual = Blend.overlay (Color.parseHex "#b83d31") (Color.parseHex "#0da671")
      let expected = Color.parseHex "#784f2b"
      Expect.equal actual expected "The blend should equal"
    }

    test "optics" {
      Expect.equal
        (Colors.red ^. Color.red)
        255uy
        "The red channel optic should work"

      Expect.equal
        (Colors.green ^. Color.green)
        255uy
        "The green channel optic should work"

      Expect.equal
        (Colors.red ^. Color.blue)
        0uy
        "The blue channel optic should work"

      Expect.equal
        (Colors.blue ^. Color.blue)
        255uy
        "The blue channel optic should work"

      Expect.equal
        (Colors.blue ^. Color.alpha)
        255uy
        "The alpha channel optic should work"
    }

  ]

[<EntryPoint>]
let main argv =
  runTestsWithCLIArgs [] argv tests
