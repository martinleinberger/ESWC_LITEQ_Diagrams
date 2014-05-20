#r "LITEQ.dll"
#r @"C:\Users\Martin\Documents\Visual Studio 2012\Projects\ESWC_Diagrams\packages\FSPowerPack.Parallel.Seq.Community.3.0.0.0\Lib\Net40\FSharp.PowerPack.Parallel.Seq.dll"
#load @"C:/Users/Martin/Documents/Visual Studio 2012/Projects/ESWC_Diagrams/packages/FSharp.Charting.0.90.6/FSharp.Charting.fsx"


open Uniko.Liteq
open FSharp.Charting
open Microsoft.FSharp.Collections

type Store = RDFStore< @"C:\Users\Martin\Documents\Visual Studio 2012\Projects\ESWC_Diagrams\ESWC_Diagrams\liteq_default.ini" >

// Creates a chart comparing number of songs to the number of artists that made this number of songs
Store.NPQL().``mo:MusicArtist``.``<-``.``foaf:made``.Extension
|> Seq.groupBy (fun artist -> Seq.length artist.``foaf:made``)
|> Seq.sortBy fst
|> Seq.map (fun (x, y) -> x, Seq.length y)
|> Seq.toList
|> Chart.Line

// Creates a pie chart of the tags contained in the store (careful, takes a very long time)
let toString (tag : Store.``tags:Tag``.Intension) = tag.``tags:tagName`` |> String.concat(" ")

let update (m : Map<_, _>) i = 
    if m.ContainsKey i then m.Add(i, m.[i] + 1)
    else m.Add(i, 1)

Store.NPQL().``mo:Record``.Extension
|> PSeq.fold (fun accumulator music_record -> 
    music_record.``tags:taggedWithTag``
    |> PSeq.map toString
    |> PSeq.fold update accumulator) Map.empty<string, int>
|> Map.toSeq
|> PSeq.sortBy (fun x -> (snd x) * -1)
|> Seq.take 10
|> Seq.toList
|> Chart.Pie