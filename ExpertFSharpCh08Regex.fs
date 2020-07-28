// Learn more about F# at http://fsharp.org

open System
open System.IO  
open System.Xml 
open System.Text.RegularExpressions 

open XML2Scene 
open RegexdotNet 
open RegexFSharp 


[<EntryPoint>]
let main argv =

    let samplestring = "This is a string"  
    printfn "regextest = %A " (samplestring =~ "(is )+")  
    printfn "regex1 = %A " (regex(" ").Split("This is a string"))  
    printfn "regex1 = %A " (regex(@"\s+").Split("I'm a little     teapot"))  
    printfn "regex1 = %A " (regex(@"\s+").Split("I'm a little \t\t\n\t\n\t teapot")) 
    printfn " "  
    let m = regex("joe").Match("maryjoewashere") 
    if m.Success then printfn "Matched at position %d" m.Index   
    let text2 = "was a dark and stormy night"  
    let t2 = regex(@"\w+").Replace(text2, "WORD") 

    let entry = @"
       Jolly Jethro
       13 Kings Parade 
       Cambridge, Cambs CB2 1TJ"

    let r = AddrBlk.Match(entry) 
    printfn "city = %A " r.Groups.["city"].Value  
    printfn "county = %A " r.Groups.["county"].Value  
    printfn "pcode = %A " r.Groups.["pcode"].Value 
    printfn " "  
    
// uses active pattern in the match 
    printfn "pcode = %A " (
        match "This is a string" with 
        | IsMatch "(?i)HIS" -> "yes, it matched"
        | IsMatch "ABC" -> "this would not match"
        | _ -> "nothing matched") 

    printfn "firstAndSecondWord = %A " (firstAndSecondWord "This is a super string")  
    printfn "firstAndSecondWord2 = %A " (firstAndSecondWord2 "This is a super string") 
    printfn " "  

    let resP = PhoneRegex().Match("425-123-2345") 
//    printfn "AreaCode = %A" resP.AreaCode.Value 
    printfn "AreaCode = %A" resP.Groups.["AreaCode"].Value   
    printfn " " 

    let inp = """<?xml version="1.0" encoding="utf-8" ?>
    <Scene>
        <Composite> 
        <Circle radius='2' x='1' y='0'/> 
        <Composite>  
         <Circle radius='2' x='4' y='0'/> 
         <Square side='2' left='-3' top='0'/> 
        </Composite>      
        <Ellipse  top='2' left='-2' width='3' height='4'/>     
        </Composite>     
    </Scene>                           
    """
    // fsi.AddPrinter(fun (x:XmlNode) -> x.OuterXml) 
    // fsi.AddPrinter(fun (r:RectangleF) -> sprintf "[%A,%A,%A,%A]" r.Left r.Top r.Width r.Height) 

    let doc = XmlDocument() 
    doc.LoadXml(inp) 
    printfn "doc.ChildNodes = %A" doc.ChildNodes  
    printfn "doc.ChildNodes.Item(1) = %A" <| doc.ChildNodes.Item(1)  
    printfn "doc.ChildNodes.Item(1).ChildNodes.Item(0) = %A" <| doc.ChildNodes.Item(1).ChildNodes.Item(0)  
    printfn "doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0)  = %A" <| doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0) 
    printfn "doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0).Attributes  = %A" <| doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0).Attributes  
    printfn " "  

    printfn "extractScenes doc  = %A" <| extractScenes doc  

    printfn " "  
    printfn "All finished from ExpertF#Ch08Regex" 
    0 // return an integer exit code
