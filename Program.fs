open System
open System.IO
open CsvHelper
open Newtonsoft.Json
open System.Collections.Generic

module Domain = 

    type Car() = 
        member val Id : string = "" with get,set
        member val Motor : string = "" with get,set
        member val Price : string = "" with get,set
        member val Doors: string = "" with get,set
        member val Color: string = "" with get,set
        member val MaxSpeed: string = "" with get,set
    
    type Bike() = 
        member val Id: string = "" with get,set
        member val Price: string = "" with get,set
        member val Color: string = "" with get,set
        member val MaxSpeed: string = "" with get,set

module CsvFiles = 
    let getPaths directory = 
        let files = Directory.GetFiles(directory,"*.csv")
        if files.Length > 0 then
            Some(files)
        else
            None

    let getFileName filePath = 
        Path.GetFileNameWithoutExtension(filePath)

module DomainMapper = 
    let private mapFromFile resultType filePath = 
        use file = File.OpenText(filePath)
        let csvReader = new CsvReader(file)
        csvReader.GetRecords(resultType) |> List.ofSeq
    
    let mapDomain filePath = 
        let fileName = CsvFiles.getFileName filePath
        match fileName with 
        | "Car" -> Some(mapFromFile typeof<Domain.Car> filePath)
        | "Bike" -> Some(mapFromFile typeof<Domain.Bike> filePath)
        |_ -> None


[<EntryPoint>]
let main argv =
    let filePaths = CsvFiles.getPaths "csv"
    let psObjectSimulator = new List<obj list option>()
    if filePaths.IsSome then
        filePaths.Value |> Array.iter((fun filePath -> psObjectSimulator.Add(DomainMapper.mapDomain filePath)))
    else
        printfn "There are no results"  
    0 
