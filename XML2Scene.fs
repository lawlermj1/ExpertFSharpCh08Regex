//  XML2Scene.fs 

module XML2Scene 

open System.Xml 
open System.Drawing  

type Scene = 
    | Ellipse of RectangleF 
    | Rect of RectangleF  
    | Composite of Scene List    

    static member Circle(center : PointF, radius) = 
        Ellipse ( RectangleF ( center.X - radius, center.Y - radius, radius * 2.0f, radius * 2.0f )) 
    static member Square( left, top, side ) = 
        Rect ( RectangleF ( left, top, side, side )) 

let extractFloat32 attrName (attribs : XmlAttributeCollection) = 
    float32 (attribs.GetNamedItem(attrName).Value) 
let extractPointF (attribs : XmlAttributeCollection) = 
    PointF( extractFloat32 "x" attribs, extractFloat32 "y" attribs) 
let extractRectangleF (attribs : XmlAttributeCollection) = 
    RectangleF( extractFloat32 "left" attribs, extractFloat32 "top" attribs, 
                extractFloat32 "width" attribs, extractFloat32 "height" attribs )   

let rec extractScene (node : XmlNode) = 
    let attribs = node.Attributes 
    let childNodes = node.ChildNodes
    match node.Name with 
    | "Circle" -> 
        Scene.Circle(extractPointF(attribs), extractFloat32 "radius" attribs) 
    | "Ellipse" -> 
        Scene.Ellipse(extractRectangleF(attribs)) 
    | "Rectangle" -> 
        Scene.Rect(extractRectangleF(attribs)) 
    | "Square" -> 
        Scene.Square(extractFloat32 "left" attribs, extractFloat32 "top" attribs, extractFloat32 "side" attribs) 
    | "Composite" -> 
        Scene.Composite [for child in childNodes -> extractScene(child)] 
    | _ -> failwithf "unable to convert XML '%s'" node.OuterXml 

let extractScenes (doc : XmlDocument) = 
    [for node in doc.ChildNodes do 
        if node.Name = "Scene" then 
            yield (Composite 
                [for child in node.ChildNodes -> extractScene(child)])]
