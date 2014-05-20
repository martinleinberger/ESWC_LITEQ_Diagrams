#r "LITEQ.dll"

open Uniko.Liteq

type Store = RDFStore< @"C:\Users\Martin\Documents\Visual Studio 2012\Projects\ESWC_Diagrams\ESWC_Diagrams\liteq_default.ini" >


// Exploration
Store.NPQL().``mo:MusicArtist``.v.``mo:SoloMusicArtist``.``->``.``foaf:based_near``.``geo:SpatialThing``

// Type definitions
type MusicArtist = Store.``mo:MusicArtist``.Intension

// Data querying
let performers = Store.NPQL().``foaf:Group``.v.``mo:MusicGroup``.``->``.``mo:member``.``foaf:Agent``.v.``mo:Performer``.Extension
for performer in performers do
    printfn "%A made %A" (performer.``foaf:name`` |> String.concat(" ")) performer.``foaf:made``


// Persistence
let newBand = new MusicArtist("http://mybandurl")
newBand.``foaf:name`` <- ["MyCoolBandName"]

