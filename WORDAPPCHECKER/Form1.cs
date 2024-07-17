using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text; 

namespace WordFileChecker
{
    public partial class Form1 : Form
    {
        private string controlFolderPath;
        private string selectedPercentage;
        private string selectedColor;
        private string selectedFontPercentage;
        private string selectedFontColor;
        private string selectedFontSize;
        private string selectedFontSizePercentage;
        private string selectedFontFamily;
        private int minParagraphCount;
        private int maxParagraphCount;
        private bool isDialogShown = false;

        private List<string> evaluations;

        public Form1()
        {
            InitializeComponent();
            InitializeCheckedListBox();
            InitializeEventHandlers();
            evaluations = new List<string>();
            btnCompareFiles.Text = "Process File"; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void InitializeEventHandlers()
        {
            btnBrowseControlFile.Click += btnBrowseControlFile_Click;
            btnBrowseControlFolder.Click += btnBrowseControlFolder_Click;
            btnCompareFiles.Click += btnProcessFile_Click; 
            btnAbout.Click += btnAbout_Click;
            btnSettings.Click += btnSettings_Click;
            btnBrowseFile.Click += btnBrowseFile_Click;
            btnProcessFile.Click += btnProcessFile_Click;
            checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
        }

        private void InitializeCheckedListBox()
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Items.Add("Background Color");
            checkedListBox1.Items.Add("Font Color");
            checkedListBox1.Items.Add("Font size");
            checkedListBox1.Items.Add("Font");
            checkedListBox1.Items.Add("Count the paragraphs");
            checkedListBox1.Items.Add("Image");
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (isDialogShown)
            {
                return;
            }

            isDialogShown = true;
            bool showDialogResult = false;

            if (checkedListBox1.Items[e.Index].ToString() == "Background Color" && e.NewValue == CheckState.Checked)
            {
                using (OptionsForm optionsForm = new OptionsForm())
                {
                    showDialogResult = optionsForm.ShowDialog() == DialogResult.OK;
                    if (showDialogResult)
                    {
                        selectedPercentage = optionsForm.SelectedPercentage;
                        selectedColor = optionsForm.SelectedColor;
                        if (!string.IsNullOrEmpty(txtFilePath.Text))
                        {
                            ProcessWordDocument(txtFilePath.Text, selectedColor, double.Parse(selectedPercentage.TrimEnd('%')), "Background");
                        }
                    }
                }
            }
            else if (checkedListBox1.Items[e.Index].ToString() == "Font Color" && e.NewValue == CheckState.Checked)
            {
                using (OptionsForm1 optionsForm1 = new OptionsForm1())
                {
                    showDialogResult = optionsForm1.ShowDialog() == DialogResult.OK;
                    if (showDialogResult)
                    {
                        selectedFontPercentage = optionsForm1.SelectedFontPercentage;
                        selectedFontColor = optionsForm1.SelectedFontColor;
                        if (!string.IsNullOrEmpty(txtFilePath.Text))
                        {
                            ProcessWordDocument(txtFilePath.Text, selectedFontColor, double.Parse(selectedFontPercentage.TrimEnd('%')), "Font");
                        }
                    }
                }
            }
            else if (checkedListBox1.Items[e.Index].ToString() == "Font size" && e.NewValue == CheckState.Checked)
            {
                using (OptionsForm2 optionsForm2 = new OptionsForm2())
                {
                    showDialogResult = optionsForm2.ShowDialog() == DialogResult.OK;
                    if (showDialogResult)
                    {
                        selectedFontSize = optionsForm2.SelectedFontSize;
                        selectedFontSizePercentage = optionsForm2.SelectedPercentage;
                        if (!string.IsNullOrEmpty(txtFilePath.Text))
                        {
                            ProcessFontSize(txtFilePath.Text, int.Parse(selectedFontSize), double.Parse(selectedFontSizePercentage));
                        }
                    }
                }
            }
            else if (checkedListBox1.Items[e.Index].ToString() == "Font" && e.NewValue == CheckState.Checked)
            {
                using (OptionsForm3 optionsForm3 = new OptionsForm3())
                {
                    showDialogResult = optionsForm3.ShowDialog() == DialogResult.OK;
                    if (showDialogResult)
                    {
                        selectedFontFamily = optionsForm3.SelectedFontFamily;
                        selectedFontPercentage = optionsForm3.SelectedPercentage;
                        if (!string.IsNullOrEmpty(txtFilePath.Text))
                        {
                            ProcessFontFamily(txtFilePath.Text, selectedFontFamily, double.Parse(selectedFontPercentage));
                        }
                    }
                }
            }
            else if (checkedListBox1.Items[e.Index].ToString() == "Count the paragraphs" && e.NewValue == CheckState.Checked)
            {
                using (OptionsForm4 optionsForm4 = new OptionsForm4())
                {
                    showDialogResult = optionsForm4.ShowDialog() == DialogResult.OK;
                    if (showDialogResult)
                    {
                        minParagraphCount = optionsForm4.MinParagraphCount;
                        maxParagraphCount = optionsForm4.MaxParagraphCount;
                        if (!string.IsNullOrEmpty(txtFilePath.Text))
                        {
                            ProcessParagraphCount(txtFilePath.Text, minParagraphCount, maxParagraphCount);
                        }
                    }
                }
            }
            else if (checkedListBox1.Items[e.Index].ToString() == "Image" && e.NewValue == CheckState.Checked)
            {
                if (!string.IsNullOrEmpty(txtFilePath.Text))
                {
                    ProcessImageCheck(txtFilePath.Text);
                }
            }

            if (!showDialogResult)
            {
                checkedListBox1.ItemCheck -= checkedListBox1_ItemCheck;
                checkedListBox1.SetItemChecked(e.Index, false);
                checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            }

            isDialogShown = false;
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Word Files|*.doc;*.docx"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnBrowseControlFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Word Files|*.doc;*.docx"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnBrowseControlFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                controlFolderPath = folderBrowserDialog.SelectedPath;
                txtFilePath.Text = controlFolderPath;
            }
        }

        private void btnProcessFile_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;

            if (string.IsNullOrEmpty(filePath))
            {
                MessageBox.Show("Please select a file.");
                return;
            }

            evaluations.Clear();

            if (checkedListBox1.GetItemChecked(0))
            {
                if (selectedColor != null && selectedPercentage != null)
                {
                    ProcessWordDocument(filePath, selectedColor, double.Parse(selectedPercentage.TrimEnd('%')), "Background");
                }
                else
                {
                    MessageBox.Show("Please select options for Background Color.");
                    return;
                }
            }
            if (checkedListBox1.GetItemChecked(1))
            {
                if (selectedFontColor != null && selectedFontPercentage != null)
                {
                    ProcessWordDocument(filePath, selectedFontColor, double.Parse(selectedFontPercentage.TrimEnd('%')), "Font");
                }
                else
                {
                    MessageBox.Show("Please select options for Font Color.");
                    return;
                }
            }
            if (checkedListBox1.GetItemChecked(2))
            {
                if (selectedFontSize != null && selectedFontSizePercentage != null)
                {
                    ProcessFontSize(filePath, int.Parse(selectedFontSize), double.Parse(selectedFontSizePercentage));
                }
                else
                {
                    MessageBox.Show("Please select options for Font Size.");
                    return;
                }
            }
            if (checkedListBox1.GetItemChecked(3))
            {
                if (selectedFontFamily != null && selectedFontPercentage != null)
                {
                    ProcessFontFamily(filePath, selectedFontFamily, double.Parse(selectedFontPercentage));
                }
                else
                {
                    MessageBox.Show("Please select options for Font.");
                    return;
                }
            }
            if (checkedListBox1.GetItemChecked(4))
            {
                if (minParagraphCount > 0 && maxParagraphCount > 0)
                {
                    ProcessParagraphCount(filePath, minParagraphCount, maxParagraphCount);
                }
                else
                {
                    MessageBox.Show("Please select options for Paragraph Count.");
                    return;
                }
            }
            if (checkedListBox1.GetItemChecked(5))
            {
                ProcessImageCheck(filePath);
            }

            WriteEvaluationToNewWordFile(evaluations);
        }

        private void ProcessWordDocument(string filePath, string inputColor, double percentage, string checkType)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                var mainPart = wordDoc.MainDocumentPart;
                int countSymbolsNotColor = 0;
                int countAllSymbols = 0;

                foreach (var text in mainPart.Document.Descendants<Text>())
                {
                    foreach (var charValue in text.Text)
                    {
                        string colorToCheck = checkType == "Background" ? GetHighlightColor(text) : GetCharacterColor(text);

                        if (!colorToCheck.Equals(inputColor, StringComparison.OrdinalIgnoreCase))
                        {
                            countSymbolsNotColor++;
                        }
                        countAllSymbols++;
                    }
                }

                string evaluation = $"\nNumber of symbols with {checkType} color not '{inputColor}': {countSymbolsNotColor}";
                if (countAllSymbols > 0)
                {
                    if (percentage >= (double)countSymbolsNotColor / countAllSymbols * 100)
                    {
                        evaluation += "\nResult: 1";
                    }
                    else
                    {
                        evaluation += "\nResult: 0";
                    }
                }
                else
                {
                    evaluation += "\nResult: ;";
                }

                evaluations.Add(evaluation);
            }
        }

        private void ProcessFontSize(string filePath, int fontSize, double percentage)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                var mainPart = wordDoc.MainDocumentPart;
                int totalSymbols = 0;
                int countSymbolsNotFontSize = 0;

                foreach (var text in mainPart.Document.Descendants<Text>())
                {
                    foreach (var charValue in text.Text)
                    {
                        int charFontSize = GetCharacterFontSize(text);
                        totalSymbols++;
                        if (charFontSize != fontSize * 2)
                        {
                            countSymbolsNotFontSize++;
                        }
                    }
                }

                double actualPercentage = (double)countSymbolsNotFontSize / totalSymbols * 100;
                string evaluation = $"\nPercentage of symbols not of size {fontSize}pt: {actualPercentage}%";

                if (actualPercentage > percentage)
                {
                    evaluation += "\nResult: 0";
                }
                else
                {
                    evaluation += "\nResult: 1";
                }

                evaluations.Add(evaluation);
            }
        }

        private void ProcessFontFamily(string filePath, string fontFamily, double percentage)
        {
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                var mainPart = wordDoc.MainDocumentPart;
                int totalSymbols = 0;
                int countSymbolsNotFontFamily = 0;

                foreach (var text in mainPart.Document.Descendants<Text>())
                {
                    foreach (var charValue in text.Text)
                    {
                        string charFontFamily = GetCharacterFontFamily(text);
                        totalSymbols++;

                        if (!charFontFamily.Equals(fontFamily, StringComparison.OrdinalIgnoreCase))
                        {
                            countSymbolsNotFontFamily++;
                        }
                    }
                }

                double actualPercentage = (double)countSymbolsNotFontFamily / totalSymbols * 100;
                string evaluation = $"\nPercentage of symbols not of font family {fontFamily}: {actualPercentage}%";

                if (actualPercentage > percentage)
                {
                    evaluation += "\nResult: 0";
                }
                else
                {
                    evaluation += "\nResult: 1";
                }

                evaluations.Add(evaluation);
            }
        }

        private void ProcessParagraphCount(string filePath, int minParagraph, int maxParagraph)
        {
            int paragraphCount = CountParagraphs(filePath);
            string evaluation;

            if (paragraphCount >= minParagraph && paragraphCount <= maxParagraph)
            {
                evaluation = "Result: 1";
            }
            else
            {
                evaluation = "Result: 0";
            }
            evaluation += $"\nNumber of paragraphs in the document: {paragraphCount}";

            evaluations.Add(evaluation);
        }

        private static int CountParagraphs(string filePath)
        {
            int paragraphCount = 0;

            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
            {
                var body = doc.MainDocumentPart.Document.Body;

                foreach (var paragraph in body.Elements<Paragraph>())
                {
                    if (!string.IsNullOrWhiteSpace(paragraph.InnerText))
                    {
                        paragraphCount++;
                    }
                }
            }

            return paragraphCount;
        }

        private void ProcessImageCheck(string filePath)
        {
            bool hasImages = DocumentContainsImages(filePath);
            string evaluation = $"The document contains images: {hasImages}\nResult: {(hasImages ? 1 : 0)}";
            evaluations.Add(evaluation);
        }

        private static bool DocumentContainsImages(string filePath)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(filePath, false))
            {
                var mainPart = doc.MainDocumentPart;

                foreach (var imagePart in mainPart.ImageParts)
                {
                    if (imagePart != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static string GetCharacterFontFamily(Text text)
        {
            var run = text.Ancestors<Run>().FirstOrDefault();

            if (run != null && run.RunProperties != null && run.RunProperties.RunFonts != null)
            {
                string fontFamily = run.RunProperties.RunFonts.Ascii;

                if (!string.IsNullOrEmpty(fontFamily))
                {
                    return fontFamily;
                }
            }

            return "Unknown";
        }

        private static string GetHighlightColor(Text text)
        {
            var run = text.Ancestors<Run>().FirstOrDefault();

            if (run != null && run.RunProperties != null && run.RunProperties.Highlight != null)
            {
                return run.RunProperties.Highlight.Val;
            }

            return "None";
        }

        private static string GetCharacterColor(Text text)
        {
            var run = text.Ancestors<Run>().FirstOrDefault();

            if (run != null && run.RunProperties != null && run.RunProperties.Color != null)
            {
                string hexColor = run.RunProperties.Color.Val;
                return ConvertHexToColorName(hexColor);
            }

            return "Black";
        }

        private static int GetCharacterFontSize(Text text)
        {
            var run = text.Ancestors<Run>().FirstOrDefault();

            if (run != null && run.RunProperties != null && run.RunProperties.FontSize != null)
            {
                string fontSizeValue = run.RunProperties.FontSize.Val;
                if (int.TryParse(fontSizeValue, out int fontSize))
                {
                    return fontSize;
                }
            }

            return 24;
        }

        private static string ConvertHexToColorName(string hexColor)
        {
            var ColorMappings = new Dictionary<string, string>
            {
                { "FF0000", "Red" },
                { "00FF00", "Green" },
                { "FFFF00", "Yellow" },
                { "70AD47", "Green, Accent 6" },
                { "4472C4", "Blue, Accent 1" },
                { "5B9BD5", "Blue, Accent 5" },
                { "ED7D31", "Orange, Accent 2" },
                { "E7E6E6", "Light Gray" },
                { "00B0F0", "Light Blue" },
                { "FFA500", "Orange" },
                { "0070C0", "Blue" },
                { "002060", "Dark Blue" },
                { "7030A0", "Purple" },
                { "FFFFFF", "White" },
                { "44546A", "Blue-Gray" },
                { "A52A2A", "Brown" },
                { "000000", "Black" },
                { "C0C0C0", "Silver" },
                { "A9A9A9", "Dark Gray" },
                { "DC143C", "Crimson" },
                { "FF69B4", "Hot Pink" },
                { "FF1493", "Deep Pink" },
                { "FFC0CB", "Pink" },
                { "F00FFF", "Magenta" },
                { "A5A5A5", "Gray" },
                { "FFC000", "Gold" },
                { "DAA520", "Goldenrod" },
                { "B8860B", "Dark Goldenrod" },
                { "CD5C5C", "Indian Red" },
                { "90EE90", "Light Green" },
                { "32CD32", "Lime Green" },
                { "228B22", "Forest Green" },
                { "008000", "Dark Green" },
                { "006400", "Dark Olive Green" },
                { "808000", "Olive" },
                { "FF00FF", "Cyan" },
                { "00FFFF", "Light Cyan" },
                { "00CED1", "Dark Turquoise" },
                { "40E0D0", "Teal" },
                { "00BFFF", "Deep Sky Blue" },
                { "1E90FF", "Dodge Blue" },
                { "6495ED", "Cornflower Blue" },
                { "483D8B", "Dark Slate Blue" },
                { "6A5ACD", "Slate Blue" },
                { "7FFF00", "Chartreuse" },
                { "D2B48C", "Tan" },
                { "F0E68C", "Khaki" },
                { "BDB76B", "Dark Khaki" },
                { "F5DEB3", "Wheat" }
            };

            if (hexColor.StartsWith("#"))
            {
                hexColor = hexColor.Substring(1);
            }

            hexColor = hexColor.ToUpper();

            if (ColorMappings.ContainsKey(hexColor))
            {
                return ColorMappings[hexColor];
            }

            return "Unknown";
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Word APP Checker\nVersion 0.0 :)\nDeveloped by ნიკა თარაშვილი, თორნიკე მიხელიძე,გიორგი კეკელიძე");

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void lstStudentFiles_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void lstStudentFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void txtFilePath_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void WriteEvaluationToNewWordFile(List<string> evaluations)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = Path.Combine(desktopPath, $"შეფასება.docx");

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(fileName, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                foreach (string evaluation in evaluations)
                {
                    Paragraph para = body.AppendChild(new Paragraph());
                    Run run = para.AppendChild(new Run());
                    run.AppendChild(new Text(evaluation));
                }
            }

            MessageBox.Show($"შეფასება იქნება მოცემული ვორდის ფაილში {fileName}");
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }
    }
}
