using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

namespace PlaceGroup
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Lab1PlaceGroup : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            // Hello github
            //Get application and document objects
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;
            if (doc == null)
                return Result.Cancelled;

            //Define a Reference object to accept the pick result.
            Reference pickedRef = null;

            //Pick a group
            Selection sel = uiApp.ActiveUIDocument.Selection;
            if (sel == null)
                return Result.Cancelled;
            pickedRef = sel.PickObject(ObjectType.Element, "Please select a group");
            if (pickedRef == null)
                return Result.Cancelled;
            Element elem = doc.GetElement(pickedRef);
            if (elem == null)
                return Result.Cancelled;
            Group group = elem as Group;

            //Pick a point
            XYZ point = sel.PickPoint("Please pick a point to place group");
            if (point == null)
                return Result.Cancelled;

            //Place the group
            Transaction trans = new Transaction(doc);
            trans.Start("Lab");
            doc.Create.PlaceGroup(point, group.GroupType);
            trans.Commit();

            return Result.Succeeded;
        } // end of Execute

    } // end of class Lab1PlaceGroup
} // end of namespace PlaceGroup