// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using O2.Core.CIR.CirObjects;
using O2.Core.CIR.CirUtils;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.Filters;
using O2.DotNetWrappers.Windows;
using O2.Kernel.Interfaces.CIR;

namespace O2.Core.CIR.Ascx
{
    public partial class ascx_CirDataViewer : UserControl
    {        
        public ascx_CirDataViewer()
        {
            cirDataAnalysis = new CirDataAnalysis();
            InitializeComponent();
        }

        private void ascx_CirDataViewer_Load(object sender, EventArgs e)
        {
            onLoad();
        }

        

        private void laNumberOfClasses_Click(object sender, EventArgs e)
        {
            showLoadedClasses();
        }

        private void laNumberOfMethods_Click(object sender, EventArgs e)
        {
            showLoadedFunctions();
        }

        private void functionsViewer__onDrop(object droppedObject)
        {
            handleDrop(droppedObject);
        }
        

        private void btViewCirClasses_Click(object sender, EventArgs e)
        {
            showLoadedClasses();
        }

        private void btViewCirFunctions_Click(object sender, EventArgs e)
        {
            showLoadedFunctions();
        }

        private void btFunctionInfo_Click(object sender, EventArgs e)
        {                     
            gbSelectedItemInfo.Visible = !gbSelectedItemInfo.Visible;
            showFunctionInformation(functionsViewer.selectedFilteredSignature);
            gbCirTrace.Visible = false;
        }

        private void btViewSmartTrace_Click(object sender, EventArgs e)
        {
            gbCirTrace.Visible = !gbCirTrace.Visible;
            gbSelectedItemInfo.Visible = false;
        }

        private void functionsViewer__onAfterSelect(object selectedItem)
        {
            showFunctionInformation(selectedItem);
            //this.invokeOnThread(
             //   ()=>
              //      {
                        //if (selectedItem)
            //var signatureToProcess = "";            
            if (selectedItem is List<FilteredSignature>) // it is a class
            {
                var selectedItems = (List<FilteredSignature>)selectedItem;
                if (selectedItems.Count > 0) 
                {
                    var filteredSignature = (FilteredSignature) selectedItems[0];
                    ICirFunction cirFunction = null;
                    //first check if we have a match on sSignature then on sOriginalSignature
                     if (cirDataAnalysis.dCirFunction_bySignature.ContainsKey(filteredSignature.sSignature))
                         cirFunction = cirDataAnalysis.dCirFunction_bySignature[filteredSignature.sSignature];
                    else if(cirDataAnalysis.dCirFunction_bySignature.ContainsKey(filteredSignature.sOriginalSignature))
                        cirFunction = cirDataAnalysis.dCirFunction_bySignature[filteredSignature.sOriginalSignature];
                if (cirFunction!=null) 
                    if (cirFunction.ParentClass != null)
                        ViewHelpers.raiseSourceCodeReferenceEvent(cbShowLineInSourceFile.Checked,cirFunction.ParentClass, true /* remapLineNumber */);
                    else
                    {
                    }                          
            }
            else if (selectedItem is FilteredSignature)  // it is  a function
            {
                var filteredSignature = ((FilteredSignature) selectedItem);
                var signatureToProcess = "";
                //first check if we have a match on sSignature then on sOriginalSignature
                if (cirDataAnalysis.dCirFunction_bySignature.ContainsKey(filteredSignature.sSignature))
                    signatureToProcess = filteredSignature.sSignature;
                else if(cirDataAnalysis.dCirFunction_bySignature.ContainsKey(filteredSignature.sOriginalSignature))
                    signatureToProcess = filteredSignature.sOriginalSignature;
                if (signatureToProcess != "")
                    ViewHelpers.raiseSourceCodeReferenceEvent(cbShowLineInSourceFile.Checked,
                                                              cirDataAnalysis.dCirFunction_bySignature[
                                                                  signatureToProcess], true /* remapLineNumber */);
            }

                        
            
                //O2Forms.SetFocusOnControl(functionsViewer, 200);
                O2Forms.SetFocusOnControl(this, 400);
                
            }




            //     })
            
            //ascx_FunctionCalls.
            //showSelectedItemDetails(selectedItem);
        }

        private void functionsViewer__onDoubleClick(object selectedItem)
        {
            showFunctionInformationOnNewWindow(selectedItem);
        }
        

        private void btCreateO2AssessmentWithCallFlowTraces_Click(object sender, EventArgs e)
        {
            if (functionsViewer.selectedFilteredSignatures.Count > 0)
            {
                createO2AssessmentWithFunctionsSignatures(functionsViewer.selectedFilteredSignatures);
            }
            else
                createO2AssessmentWithCallFlowTraces();
        }
        

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void btDeleteAllLoadedData_Click(object sender, EventArgs e)
        {
            deleteAllLoadedData();
        }

        

        
        private void llCreateRulesWith_LostSinks_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getFunctionsListWith_LostSinks(), DragDropEffects.Copy);
        }        

        private void llCreateRulesWith_LostSources_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getFunctionsListWith_LostSources(), DragDropEffects.Copy);
        }

        private void llDrag_AllFunctions_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getAllFunctions(), DragDropEffects.Copy);
        }


        private void llDrag_CurrentFilteredFunctions_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getFunctionsListWith_CurrentFilteredFunctions(), DragDropEffects.Copy);
        }        

        private void showRulesCreationTools_Click(object sender, EventArgs e)
        {
            gbRulesCreationTools.Visible = !gbRulesCreationTools.Visible;
        }


        private void rbOnlyShowFunctionsWithControlFlowGraphs_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOnlyShowFunctionsWithControlFlowGraphs.Checked)
            {
                cirDataAnalysis.onlyShowFunctionsWithCallersOrCallees = false;
                cirDataAnalysis.onlyShowFunctionsOrClassesWithControlFlowGraphs = true;
                cirDataAnalysis.onlyShowExternalFunctionsThatAreInvokedFromCFG = false;
                showLoadedFunctions();
            }
        }

        private void rbShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbShowAll.Checked)
            {
                cirDataAnalysis.onlyShowFunctionsWithCallersOrCallees = false;
                cirDataAnalysis.onlyShowFunctionsOrClassesWithControlFlowGraphs = false;
                cirDataAnalysis.onlyShowExternalFunctionsThatAreInvokedFromCFG = false;
                showLoadedFunctions();
            }
        }

        private void rbOnlyShowExternalFunctionsThatAreInvokedFromCFG_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOnlyShowExternalFunctionsThatAreInvokedFromCFG.Checked)
            {
                cirDataAnalysis.onlyShowFunctionsWithCallersOrCallees = false;
                cirDataAnalysis.onlyShowFunctionsOrClassesWithControlFlowGraphs = false;
                cirDataAnalysis.onlyShowExternalFunctionsThatAreInvokedFromCFG = true;
                showLoadedFunctions();
            }
            
        }

        private void rbShowOnlyFunctionsWithCallersOrCallees_CheckedChanged(object sender, EventArgs e)
        {
            if (rbShowOnlyFunctionsWithCallersOrCallees.Checked)
            {
                cirDataAnalysis.onlyShowFunctionsWithCallersOrCallees = true;
                cirDataAnalysis.onlyShowFunctionsOrClassesWithControlFlowGraphs = false;
                cirDataAnalysis.onlyShowExternalFunctionsThatAreInvokedFromCFG = false;
                showLoadedFunctions();
            }
        }

        private void btDragFunctions_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop(getAllFunctions(), DragDropEffects.Copy);
        }

        private void functionsViewer__onItemDrag(object objectToDrag)
        {
            handleOnItemDrag(objectToDrag);
        }

        private void gbSelectedItemInfo_SizeChanged(object sender, EventArgs e)
        {
            scrollBarVerticalSize.Maximum = gbSelectedItemInfo.Height + 20;            
            scrollBarVerticalSize.Value = gbSelectedItemInfo.Height;

            scrollBarHorizontalSize.Maximum = gbSelectedItemInfo.Width + 20;
            scrollBarHorizontalSize.Value = gbSelectedItemInfo.Width;
        }

        private void scrollBarVerticalSize_Scroll(object sender, ScrollEventArgs e)
        {            
            var difference = gbSelectedItemInfo.Height - scrollBarVerticalSize.Value;
            gbSelectedItemInfo.Height = scrollBarVerticalSize.Value;
            gbSelectedItemInfo.Top += difference;
        }

        private void scrollBarHorizontalSize_Scroll(object sender, ScrollEventArgs e)
        {
            var difference = gbSelectedItemInfo.Width - scrollBarHorizontalSize.Value;
            gbSelectedItemInfo.Width = scrollBarHorizontalSize.Value;
            gbSelectedItemInfo.Left  += difference;
        }

        private void btSaveAsCirDataFile_Click(object sender, EventArgs e)
        {            
            saveAsCirDataFile();
        }

        private void llLoadCurrentAssembly_Click(object sender, EventArgs e)
        {            
            loadFile(DI.config.ExecutingAssembly);
        }     

    }
}
