module Chromatic.Demo.ColorEditor

open Fable.Core.JsInterop
open Feliz
open Chromatic
open Chromatic.Optics
open Aether
open Aether.Operators
open Chromatic.Demo.ColorBox

type Props =
  {
    InitialValue : Color
    OnChange : Color -> Unit
  }

type private State =
  {
    Color : Color
    Hex : string
  }

let colorEditor =
  React.functionComponent (fun (props : Props) ->
    let (state, setState) =
      React.useState
        {
          Color = props.InitialValue
          Hex = Color.hex props.InitialValue
        }

    let updateColor (nextColor : Color) =
      setState
        { state with
            Color = nextColor
            Hex = Color.hex nextColor }

      props.OnChange nextColor

    let channelEditor (name : string) (lens : Lens<_, uint8>) =
      let c = state.Color ^. lens

      Html.li
        [
          prop.classes [ "form-group"; "list-group-item" ]
          prop.children
            [
              Html.label [ Html.text (sprintf "%s: %i" name c) ]
              Html.input
                [
                  prop.type'.range
                  prop.classes [ "form-control-range" ]
                  prop.min 0
                  prop.max 255
                  prop.value (int c)
                  prop.onChange
                    (fun (e : Browser.Types.Event) ->
                      let i : int = !!(e.target?value)
                      let b = byte i

                      let nextColor = state.Color |> b ^= lens

                      updateColor nextColor)
                ]
            ]
        ]

    Html.form
      [
        prop.onSubmit (fun e -> e.preventDefault ())
        prop.children
          [
            Html.ul
              [
                prop.classes [ "list-group"; "list-group-flush" ]
                prop.children
                  [
                    channelEditor "Red" Color.red
                    channelEditor "Green" Color.green
                    channelEditor "Blue" Color.blue
                  ]
              ]

            colorBox state.Color

            Html.div
              [
                prop.classes [ "input-group"; "mb-3"; "mt-2" ]
                prop.children [
                  Html.div
                    [
                      prop.classes [ "input-group-prepend" ]
                      prop.children [
                        Html.span [
                          prop.classes [ "input-group-text" ]
                          prop.children [
                            Html.text "Hex"
                          ]
                        ]
                      ]
                    ]

                  Html.input
                    [
                      prop.type'.text
                      prop.classes [ "form-control"; "text-monospace" ]
                      prop.value state.Hex
                      prop.onChange (fun maybeHex ->
                        match Color.tryParseHex maybeHex with
                        | Some color ->
                          setState {
                            state with
                              Color = color
                              Hex = maybeHex
                          }
                        | None ->
                          setState {
                            state with Hex = maybeHex
                          }
                      )
                    ]
                ]
              ]

            let complementary = Color.complementary state.Color
            let temperature =  Color.fromTemperature (Color.temperature state.Color)

            Html.dl
              [
                Html.dt "Complementary"
                Html.dd
                  [
                    prop.style [ style.cursor "pointer" ]
                    prop.onClick (fun e ->
                      e.preventDefault ()

                      updateColor complementary
                    )
                    prop.children [ colorBox complementary ]
                  ]
                Html.dt "Temperature"
                Html.dd
                  [
                    prop.style [ style.cursor "pointer" ]
                    prop.onClick (fun e ->
                      e.preventDefault ()

                      updateColor temperature
                    )
                    prop.children [ colorBox temperature ]
                  ]
              ]
          ]
      ]
  )
