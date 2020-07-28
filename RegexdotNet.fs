//  RegexdotNet.fsx

module RegexdotNet 

open System 
open System.IO  
open System.Text.RegularExpressions
// There is some overlap between System.Text.RegularExpressions and FSharp.Text.RegexProvider
// So don't open in same module. 
// open FSharp.Text.RegexProvider 

// functions 
// splits line by commas 
let splitLine (line : string) = line.Split ',' |> Array.map (fun s -> s.Trim())  

// parse date 
let date x = DateTime.Parse(x) 

// parses a string into a list? 
let parseEmployee (line : string) = 
    match splitLine line with 
    | [|last; first; startDate; title |] -> 
        last, first, System.DateTime.Parse(startDate), title
    | _ -> 
        failwithf "invalid employee format: '%s'" line 

// reads a file, and parses each line 
let readEmployees (fileName : string) = 
    fileName |> File.ReadLines |> Seq.map parseEmployee 

// reads a file and version from HTTP request 
let parseHttpRequest line = 
    let result = Regex.Match(line, @"Get (.?) HTTP/1\.([01])$") 
    let file = result.Groups.[1].Value 
    let version = result.Groups.[2].Value 
    file, version 

// regex auxilliary 
let regex s = Regex(s) 
// let (=~) (s : string) (re : Regex) = re.IsMatch(s) 
let (=~)  s re = Regex(re).IsMatch(s) 
//let (<>~) s re  = not (s =~ (regex re) )

// parse address block with System.Text.RegularExpressions 
let AddrBlk = Regex  @"(?<=\n)\s*(?<city>[^\n]+)\s*,\s*(?<county>\w+)\s+(?<pcode>.{3}\s*.{3}).*$" 
// Now with FSharp.Text.RegexProvider   
// type AddrBlk = Regex< @"(?<=\n)\s*(?<city>[^\n]+)\s*,\s*(?<county>\w+)\s+(?<pcode>.{3}\s*.{3}).*$" >

// creating an active pattern function  
let (|IsMatch|_|) (re:string) (inp:string) = 
    if Regex(re).IsMatch(inp) then Some() else None 

// type W1W2 = Regex< @"(?<word1>\w+)\s+(?<word2>\w+)" > 
// uses standard regex to extract values 
let firstAndSecondWord (inp:string) =
    let re = Regex @"(?<word1>\w+)\s+(?<word2>\w+)" 
//    type re = Regex<"(?<word1>\w+)\s+(?<word2>\w+)">   
    let results = re.Match(inp)
//    let results = W1W2.Match(inp)    
    if results.Success then 
        Some (results.Groups.["word1"].Value, results.Groups.["word2"].Value) 
    else 
        None 

// uses standard regex to extract values using more typical functions 
let (?) (results:Match) (name:string) = 
    results.Groups.[name].Value 

let firstAndSecondWord2 (inp:string) =
    let re = Regex @"(?<word1>\w+)\s+(?<word2>\w+)" 
    let results = re.Match(inp)
    if results.Success then 
        Some (results ? word1, results ? word2) 
    else 
        None 

