//  ExpertFSharpCh08Regex.fsx

open System 
open System.IO  
open System.Xml 
open System.Text.RegularExpressions 


let parseHttpRequest line = 
    let result = Regex.Match(line, @"Get (.?) HTTP/1\.([01])$") 
    let file = result.Groups.[1].Value 
    let version = result.Groups.[2].Value 
    file, version 
let regex s = Regex(s) ;; 
let (=~) s (re:Regex) = re.IsMatch(s) ;; 
let (<>~) s (re:Regex) = not (s =~ re) ;; 
let samplestring = "This is a string" ;;
if samplestring =~ regex "his" then printfn "A Match! " ;; 
"This is a string" =~ regex "(is )+" ;;
regex(" ").Split("This is a string");;
regex(@"\s+").Split("I'm a little     teapot");;
regex(@"\s+").Split("I'm a little \t\t\n\t\n\t teapot");;
let m = regex("joe").Match("maryjoewashere");;
if m.Success then printfn "Matched at position %d" m.Index ;; 
let text2 = "was a dark and stormy night" ;;
let t2 = regex(@"\w+").Replace(text2, "WORD");;

let entry = @"
Jolly Jethro
13 Kings Parade 
Cambridge, Cambs CB2 1TJ
"
let re = 
    regex @"(?<=\n)\s*(?<city>[^\n]+)\s*,\s*(?<county>\w+)\s+(?<pcode>.{3}\s*.{3}).*$" 
let r = re.Match(entry);;
r.Groups.["city"].Value ;;
r.Groups.["county"].Value ;;
r.Groups.["pcode"].Value ;;

// creating an active pattern function  
let (|IsMatch|_|) (re:string) (inp:string) = 
    if Regex(re).IsMatch(inp) then Some() else None 

match "This is a string" with 
| IsMatch "(?i)HIS" -> "yes, it matched"
| IsMatch "ABC" -> "this would not match"
| _ -> "nothng matched"

let firstAndSecondWord (inp:string) =
    let re = regex "(?<word1>\w+)\s+(?<word2>\w+)" 
    let results = re.Match(inp)
    if results.Success then 
        Some (results.Groups.["word1"].Value, results.Groups.["word2"].Value) 
    else 
        None 
firstAndSecondWord "This is a super string" ;;

let (?) (results:Match) (name:string) = 
    results.Groups.[name].Value 

let firstAndSecondWord2 (inp:string) =
    let re = regex "(?<word1>\w+)\s+(?<word2>\w+)" 
    let results = re.Match(inp)
    if results.Success then 
        Some (results ? word1, results ? word2) 
    else 
        None 
firstAndSecondWord2 "This is a super string" ;; 

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
let doc = XmlDocument();; 

doc.LoadXml(inp) ;;
doc.ChildNodes ;;
// fsi.AddPrinter(fun (x:XmlNode) -> x.OuterXml) ;; 
doc.ChildNodes ;; 
doc.ChildNodes.Item(1) ;;
doc.ChildNodes.Item(1).ChildNodes.Item(0) ;;
doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0) ;;
doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0).Attributes ;;

