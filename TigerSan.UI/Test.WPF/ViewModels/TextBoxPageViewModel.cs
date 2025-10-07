using System.IO;
using System.Text;

namespace Test.WPF.ViewModels
{
    public class TextBoxPageViewModel : BindableBase
    {
        #region 【Properties】
        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }
        private string _text = string.Empty;

        /// <summary>
        /// 文本域文本
        /// </summary>
        public string TextAreaText
        {
            get { return _textAreaText; }
            set { SetProperty(ref _textAreaText, value); }
        }
        private string _textAreaText = string.Empty;
        #endregion 【Properties】

        #region 【Ctor】
        public TextBoxPageViewModel()
        {
            InitTextBox();
        }
        #endregion 【Ctor】

        #region 【Evnets】
        #endregion 【Evnets】

        #region 【Commands】
        #endregion 【Commands】

        #region 【Functions】
        #region 初始化“文本框”
        public void InitTextBox()
        {
            var path = @"Files\text_area_text.txt";
            if (!File.Exists(path)) return;

            TextAreaText = File.ReadAllText(path, Encoding.UTF8);
        }
        #endregion
        #endregion 【Functions】
    }
}
