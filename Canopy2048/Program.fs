namespace Canopy2048

open canopy
open runner
open System
open Canopy2048.GreedyBot

module program = 

    let finished () =
        match someElement ".game-message.game-over" with
        | None -> false
        | Some(_) -> true
            
    let state () = 
        elements ".tile"
        |> List.map(fun tile ->
            let classes = tile.GetAttribute("class").Split([|' '|])
            let pointClass = classes |> Array.find(fun classs -> classs.StartsWith("tile-"))
            let point = pointClass.Split([|'-'|]).[1] |> System.Convert.ToInt32
            let rowColumnClass = classes |> Array.find(fun classs -> classs.StartsWith("tile-position-"))
            let column = rowColumnClass.Split([|'-'|]).[2] |> System.Convert.ToInt32
            let row = rowColumnClass.Split([|'-'|]).[3] |> System.Convert.ToInt32
            { Row = row; Col = column; Value = point })

    let moves = [| Up; Down; Left; Right; |]
    let rng = System.Random ()
    
    let decide (state:State) =
        moves.[rng.Next(0,4)]

    let play (move:Move) =
        match move with
        | Up -> press up
        | Down -> press down
        | Right -> press right
        | Left -> press left

    let showDecision (move:Move) = 
        printfn "%A" move
        move

    "starting a game of 2048" &&& fun _ ->

        start chrome
        url "http://gabrielecirulli.github.io/2048/"

        let rec nextMove () =
            if finished () then 
                printfn "Game over"
                ignore ()
            else          
                printfn "Thinking"
                state ()
                |> GreedyBot.decide
                |> showDecision
                |> play
                nextMove ()

        nextMove ()

    //run all tests
    run()

    printfn "press [enter] to exit"
    System.Console.ReadLine() |> ignore

    quit()