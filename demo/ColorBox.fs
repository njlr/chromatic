module Chromatic.Demo.ColorBox

open Feliz
open Chromatic

let colorBox =
  React.functionComponent (fun color ->
    Html.span
      [
        prop.style
          [
            style.display.inlineBlock
            style.width (length.percent 100)
            style.height 38
            style.borderRadius 4
            style.backgroundColor (Color.hex color)
            style.border (1, borderStyle.solid, "#ccc")
          ]
      ]
  )
