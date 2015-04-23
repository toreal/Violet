using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;

// TODO:    請依照下列步驟啟用功能區 (XML) 項目: 

// 1: 將下列程式碼區塊複製到 ThisAddin、ThisWorkbook 或 ThisDocument 類別。

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon();
//  }

// 2. 在這個類別的「功能區回呼」區域中建立回呼方法以處理使用者動作，
//    例如按一下按鈕的動作。請注意，如果您已經從功能區設計工具匯出此功能區，
//    就必須從回呼方法的事件處理常式中移除您的程式碼並修改程式碼以使用
//    功能區擴充功能 (RibbonX) 程式撰寫模型。

// 3. 指派屬性給功能區 XML 檔案中的控制標記以辨認程式碼中適當的回呼方法。

// 如需詳細資訊，請參閱 Visual Studio Tools for Office 說明中的功能區 XML 文件。


namespace keago0403
{
    [ComVisible(true)]
    public class Ribbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon()
        {
        }

        #region IRibbonExtensibility 成員

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("keago0403.Ribbon.xml");
        }

        #endregion

        #region 功能區回呼
        //在此建立回呼方法。如需加入回呼方法的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        #endregion

        #region Helper

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
