module Chromatic.Demo.App

open Feliz
open Chromatic
open Chromatic.Demo.ColorBox
open Chromatic.Demo.ColorEditor

type State =
  {
    A : Color
    B : Color
  }

let private commonHeight = length.em 32

let private wrapper classes child =
  Html.div
    [
      prop.classes classes
      prop.children [ child ]
    ]

let counter =
  React.functionComponent (fun () ->
    let (state, setState) =
      React.useState
        {
          A = Color.parseHex "#aa5697ff"
          B = Color.parseHex "#739b6cff"
        }

    Html.div
      [
        prop.classes [ "container" ]
        prop.children [
          Html.h1 [ prop.classes [ "mt-4" ]; prop.children [ Html.text "Chromatic" ] ]

          Html.div
            [
              prop.classes [ "row"; "mt-4" ]
              prop.children
                [
                  wrapper
                    [ "col-md-4" ]
                    (Html.div
                      [
                        prop.classes [ "card"; "mt-4" ]
                        prop.style [ style.minHeight commonHeight ]
                        prop.children [
                          Html.div
                            [
                              prop.classes [ "card-body" ]
                              prop.children [
                                Html.h5
                                  [
                                    prop.classes [ "card-title" ]
                                    prop.children [ Html.text "Color A" ]
                                  ]
                                colorEditor
                                  {
                                    InitialValue = state.A
                                    OnChange = (fun c -> setState { state with A = c })
                                  }
                              ]
                            ]
                        ]
                      ])

                  wrapper
                    [ "col-md-4" ]
                    (Html.div
                      [
                        prop.classes [ "card"; "mt-4" ]
                        prop.style [ style.minHeight commonHeight ]
                        prop.children [
                          Html.div
                            [
                              prop.classes [ "card-body" ]
                              prop.children [
                                Html.h5
                                  [
                                    prop.classes [ "card-title" ]
                                    prop.children [ Html.text "Color B" ]
                                  ]
                                colorEditor
                                  {
                                    InitialValue = state.B
                                    OnChange = (fun c -> setState { state with B = c })
                                  }
                              ]
                            ]
                        ]
                      ])

                  wrapper
                    [ "col-md-4" ]
                    (Html.div
                      [
                        prop.classes [ "card"; "mt-4" ]
                        prop.style [ style.minHeight commonHeight ]
                        prop.children [
                          (Html.div
                            [
                              prop.classes [ "card-body" ]
                              prop.children [
                                Html.h5
                                  [
                                    prop.classes [ "card-title" ]
                                    prop.children [ Html.text "Blends" ]
                                  ]
                                Html.dl
                                  [
                                    Html.dt "Multiply"
                                    Html.dd [ colorBox (Blend.multiply state.A state.B) ]
                                    Html.dt "Additive"
                                    Html.dd [ colorBox (Blend.additive state.A state.B) ]
                                    Html.dt "Screen"
                                    Html.dd [ colorBox (Blend.screen state.A state.B) ]
                                    Html.dt "Lighten"
                                    Html.dd [ colorBox (Blend.lighten state.A state.B) ]
                                    Html.dt "Darken"
                                    Html.dd [ colorBox (Blend.darken state.A state.B) ]
                                  ]
                              ]
                            ])
                        ]
                      ])
              ]
            ]
        ]
      ]
  )

open Browser.Dom

ReactDOM.render(counter, document.getElementById "root")
