module Chromatic.Blend

open Chromatic

let multiply (x : Color) (y : Color) =
  let channel x y =
    byte ((float x / 255.0 * float y / 255.0) * 255.0)

  {
    Red = channel x.Red y.Red
    Green = channel x.Green y.Green
    Blue = channel x.Blue y.Blue
    Alpha = channel x.Alpha y.Alpha
  }

let additive (x : Color) (y : Color) =
  let channel x y =
    byte (min (float x + float y) 255.0)

  {
    Red = channel x.Red y.Red
    Green = channel x.Green y.Green
    Blue = channel x.Blue y.Blue
    Alpha = channel x.Alpha y.Alpha
  }

let screen (x : Color) (y : Color) =
  let channel x y =
    byte ((1.0 - (1.0 - float x / 255.0) * (1.0 - float y / 255.0)) * 255.0)

  {
    Red = channel x.Red y.Red
    Green = channel x.Green y.Green
    Blue = channel x.Blue y.Blue
    Alpha = channel x.Alpha y.Alpha
  }

let overlay (x : Color) (y : Color) =
  let channel x y =
    if (float x / 255.0) < 0.5
    then
      byte ((2.0 * (float x / 255.0) * (float y / 255.0)) * 255.0)
    else
      byte ((1.0 - 2.0 * (1.0 - float x / 255.0) * (1.0 - float y / 255.0)) * 255.0)

  {
    Red = channel x.Red y.Red
    Green = channel x.Green y.Green
    Blue = channel x.Blue y.Blue
    Alpha = channel x.Alpha y.Alpha
  }

let darken (x : Color) (y : Color) =
  let channel x y =
    min x y

  {
    Red = channel x.Red y.Red
    Green = channel x.Green y.Green
    Blue = channel x.Blue y.Blue
    Alpha = channel x.Alpha y.Alpha
  }

let lighten (x : Color) (y : Color) =
  let channel x y =
    max x y

  {
    Red = channel x.Red y.Red
    Green = channel x.Green y.Green
    Blue = channel x.Blue y.Blue
    Alpha = channel x.Alpha y.Alpha
  }
