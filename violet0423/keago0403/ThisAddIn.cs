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

        internal void AddPictureContentControl(Utility _utility)
        {
            Microsoft.Office.Tools.Word.Document vstoDocument = Globals.Factory.GetVstoObject(this.Application.ActiveDocument);
            Microsoft.Office.Interop.Word.Selection selection = this.Application.Selection;
            if (selection != null && selection.Range != null)
            {
                WdSelectionType sel = selection.Type;    // inlineshape



               // if (sel == WdSelectionType.wdSelectionInlineShape)
                {
                    InlineShapes shape = vstoDocument.InlineShapes;
                    //shape[0].
                    MemoryStream ms = new MemoryStream(_utility.BitmapBytes);
                    Image _drawnimage = Image.FromStream(ms);
                    // Word.ContentControl contentControl = Globals.ThisAddIn.Application.ActiveDocument.SelectContentControlsByTitle(_utility.TagName)[0];
                    //foreach (Word.ContentControl contentControl in vstoDocument.Content.ContentControls)
                    {
                      //  WdContentControlType type = contentControl.Type;
                        //if (type == WdContentControlType.wdContentControlPicture && contentControl.Tag.Equals(_utility.TagName))
                        {

                            PictureContentControl piccontrol = vstoDocument.Controls.AddPictureContentControl(selection.Range, Guid.NewGuid().ToString());
                            // piccontrol3.Image.Save(ms, ImageFormat.Jpeg);
                            //System.Windows.Forms.MessageBox.Show(piccontrol.Tag);
                            piccontrol.Image = ScaleImage(_drawnimage, 200, 150);//Save(new Bitmap(returnImage), 270, 180, 0);


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
