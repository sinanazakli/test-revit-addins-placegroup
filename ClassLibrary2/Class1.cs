﻿using System;
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
            try
            {
                //Get application and document objects
                UIApplication   uiApp       = commandData.Application;
                Document        doc         = uiApp.ActiveUIDocument.Document;

                //Define a Reference object to accept the pick result.
                // Reference       pickedRef   = null;

                //Picker Filter
                GrooupFilter    filter      = new GrooupFilter();

                //Pick a group
                Selection       sel         = uiApp.ActiveUIDocument.Selection;
                Reference       pickedRef   = sel.PickObject(ObjectType.Element, filter, "Please select a group");
                Element         elem        = doc.GetElement(pickedRef);
                Group           group       = elem as Group;

                //Pick a point
                XYZ             point       = sel.PickPoint("Please pick a point to place group");

                //Place the group
                Transaction     trans       = new Transaction(doc);
                trans.Start("Lab");
                doc.Create.PlaceGroup(point, group.GroupType);
                trans.Commit();
            }
            catch (Autodesk.Revit.Exceptions.ArgumentNullException ex)
            {
                message = ex.Message;
                return Result.Cancelled;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
            {
                return Result.Cancelled;
            }
            return Result.Succeeded;
        } // end of Execute

    } // end of class Lab1PlaceGroup

    class GrooupFilter : ISelectionFilter
    {
        bool ISelectionFilter.AllowElement(Element elem)
        {
            return (elem.Category.Id.IntegerValue.Equals((int)BuiltInCategory.OST_IOSModelGroups));
        }

        bool ISelectionFilter.AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    } // end of GroupFilter
} // end of namespace PlaceGroup