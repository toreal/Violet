using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using Microsoft.Office.Interop.Word;
using System.IO;
using System.Drawing;
//using Chem4Word.Core;
using Microsoft.Office.Core;
using System.Diagnostics;

namespace keago0403
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO 產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion

        bool doonce = false;

        protected override object RequestService(Guid serviceGuid)
        {
            if ( !doonce)
            {
                Application.DocumentBeforeSave += Application_DocumentBeforeSave;
                //  Application.WindowSelectionChange += Application_WindowSelectionChange;
                Application.WindowBeforeDoubleClick += Application_WindowBeforeDoubleClick;
                doonce = true;
            }
            
            return base.RequestService(serviceGuid);
        }

        void Application_WindowBeforeDoubleClick(Selection sel, ref bool Cancel)
        {
           
            int n = sel.ContentControls.Count;
            Microsoft.Office.Interop.Word.ContentControl cp = sel.Range.ParentContentControl;
            
            String mytitle=null;
            String sid=null;
            if (cp!=null)
            {
                mytitle = cp.Title;
                sid = cp.ID;
            

            if (mytitle == "violet")
            {
                Debug.WriteLine("control selected "+sid);
                string xid = cp.Tag;

                Microsoft.Office.Tools.Word.Document vstoDocument = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
                CustomXMLPart xmlpart = vstoDocument.CustomXMLParts.SelectByID(xid);
                string xml=xmlpart.XML;


                Globals.Ribbons.Ribbon1.initPath(xml);

                Globals.Ribbons.Ribbon1.RibbonUI.ActivateTabMso("TabAddIns");
                
                
             
            }
            }





            //throw new NotImplementedException();
        }

        void Application_DocumentBeforeSave(Word.Document doc, ref bool SaveAsUI, ref bool Cancel)
        {
            _Document vstoDocument = Globals.Factory.GetVstoObject(this.Application.ActiveDocument) as _Document;
            //List<ControlProperties> savedControls = new List<ControlProperties>();
           // Chem4Word.Core.ControlProperties[] controls =new Chem4Word.Core.ControlProperties[100];
           // Chem4Word.Core.ControlsStorage.Store(vstoDocument, controls);
            //throw new NotImplementedException();



            //savedControls.Sort(new ControlCollectionComparer());
            //ControlsStorage.Store(doc, savedControls.ToArray());
            
        }


        internal void AddPictureContentControl(Utility _utility)
        {
            Microsoft.Office.Tools.Word.Document vstoDocument = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            Microsoft.Office.Interop.Word.Selection selection = this.Application.Selection;
            if (selection != null && selection.Range != null)
            {
                WdSelectionType sel = selection.Type;    // inlineshape



               // if (sel == WdSelectionType.wdSelectionInlineShape)
                {
                  //  InlineShapes shape = vstoDocument.InlineShapes;
                    //shape[0].
                    MemoryStream ms = new MemoryStream(_utility.BitmapBytes);




                    Image _drawnimage = Image.FromStream(ms);

                    _drawnimage.Save("temp.jpg");
                    // Word.ContentControl contentControl = Globals.ThisAddIn.Application.ActiveDocument.SelectContentControlsByTitle(_utility.TagName)[0];
                    //foreach (Word.ContentControl contentControl in vstoDocument.Content.ContentControls)
                    {
                      //  WdContentControlType type = contentControl.Type;
                        //if (type == WdContentControlType.wdContentControlPicture && contentControl.Tag.Equals(_utility.TagName))
                        {

                            //object missing = Type.Missing;
                            //Microsoft.Office.Interop.Word.ContentControl contentControl = vstoDocument.ContentControls.Add(WdContentControlType.wdContentControlPicture,
                            //                                                          ref missing);
                            //contentControl.Range.InlineShapes.AddPicture("temp.jpg", ref missing, ref missing,
                            //                                                  ref missing);


                            //contentControl.Title = "violet";
                            Microsoft.Office.Interop.Word.ContentControl cp2 = selection.Range.ParentContentControl;

                            if (cp2!= null )//update
                            {
                                

                                    
                                    CustomXMLPart xmlpart = vstoDocument.CustomXMLParts.SelectByID(cp2.Tag);
                                    xmlpart.Delete();
                                    cp2.Delete();
                                    

                                
                                

                            }
                             
                            
                            CustomXMLPart cp = vstoDocument.CustomXMLParts.Add(_utility.xml);


                            PictureContentControl piccontrol = vstoDocument.Controls.AddPictureContentControl(selection.Range, Guid.NewGuid().ToString());
                            piccontrol.Image = _drawnimage;// ScaleImage(_drawnimage, 200, 150);//Save(new Bitmap(returnImage), 270, 180, 0);
                            piccontrol.Title = "violet";
                            piccontrol.Tag = cp.Id;
                            
                        }

                    }

              //      vstoDocument.Save();
                    ms.Flush();
                    ms.Close();
                }
            }


        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}
