open System
open System.IO
open CsvHelper
open Newtonsoft.Json
open System.Collections.Generic

module Csv = 
    let getFiles directory = 
        let files = Directory.GetFiles(directory,"*.csv")
        if files.Length > 0 then Some(files) else None

    let getFileName filePath = 
        Path.GetFileNameWithoutExtension(filePath)

    let parseFile<'T> filePath = 
        use file = File.OpenText(filePath)
        let csvReader = new CsvReader(file)
        csvReader.GetRecords<'T>() 
        |> List.ofSeq

module Domain = 

    type DomainBase() =
        member val Id:string = "" with get,set

    type Car() = 
        inherit DomainBase()
        member val Motor : string = "" with get,set
        member val Price : string = "" with get,set
        member val Doors: string = "" with get,set
        member val Color: string = "" with get,set
        member val MaxSpeed: string = "" with get,set
    
    type Bike() = 
        inherit DomainBase()
        member val Id: string = "" with get,set
        member val Price: string = "" with get,set
        member val Color: string = "" with get,set
        member val MaxSpeed: string = "" with get,set

    let map filePath = 
        let fileName = Csv.getFileName filePath
        match fileName with 
        |"Car" -> 
                Some(fileName,
                    (Csv.parseFile<Car> filePath) 
                    |> Seq.cast<DomainBase>)                       
        |"Bike" -> 
                Some(fileName,
                    (Csv.parseFile<Bike> filePath) 
                    |> Seq.cast<DomainBase>)                                     
        |_ -> None

[<EntryPoint>]
let main argv =
    let files = Csv.getFiles "csv"
    if files.IsSome then
        let resultMembers  =
            files.Value
             |> Seq.map(Domain.map)
             |> Seq.filter((fun domain -> domain.IsSome)) 
             |> Seq.map((fun domain -> domain.Value))

        printfn "%A" resultMembers
    else
        printfn "There are no csv files in provided directory."  
    0 
