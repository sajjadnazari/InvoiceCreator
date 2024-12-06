using System.Globalization;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace InvoiceCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtSavePath.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txtDate.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(txtInvoiceNumber.Text) || int.TryParse(txtInvoiceNumber.Text, out int invoiceNumberIndex))
                        {
                            var transactionsPath = txtTransaction.Text;
                            var productsFilePath = txtProduct.Text;
                            var invoiceFilePath = txtSavePath.Text + $"\\Invoice {DateTime.Now.Date.Year}-{DateTime.Now.Date.Month}-{DateTime.Now.Date.Day}@{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.xlsx";
                            invoiceNumberIndex = int.Parse(txtInvoiceNumber.Text);
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var transactionAmounts = await ReadAmountsAsync(transactionsPath);
                            if (transactionAmounts?.Any() == true)
                            {
                                var products = await ReadProductsAsync(productsFilePath);
                                if (products?.Any() == true)
                                {
                                    var totalProduct = products.Sum(e => e.Quantity * e.Price);
                                    long sumTransactions = transactionAmounts.Sum(e => e.Amount);
                                    if (sumTransactions >= totalProduct)
                                        MessageBox.Show("جمع کل موجودی کالا از جمع کل مجموع تراکنش بیشتر است");
                                    else
                                        CreteInvoiceAsync(invoiceFilePath, transactionAmounts.ToList(), products.ToList(), invoiceNumberIndex, txtDate.Text);
                                }
                                else
                                    MessageBox.Show("Product Files Not found", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                                MessageBox.Show("Transaction Files (Amount) is empty ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            MessageBox.Show("Invoice number start index not valid", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                        MessageBox.Show("Date is empty", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Save output file path is not selected", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<List<Transaction>> ReadAmountsAsync(string transactionFilepath)
        {
            if (!string.IsNullOrWhiteSpace(transactionFilepath))
            {
                using (var package = new ExcelPackage(new FileInfo(transactionFilepath)))
                {
                    List<Transaction> records = new List<Transaction>();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    List<Task> tasks = new List<Task>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var currentRow = row;
                        tasks.Add(Task.Run(() =>
                        {
                            var price = int.Parse(worksheet.Cells[currentRow, 1].Text);
                            var description = worksheet.Cells[currentRow, 2].Text;
                            lock (records) { records.Add(new Transaction { Amount = price, Description = description }); }
                        }));
                    }
                    await Task.WhenAll(tasks);
                    return records;
                }
            };

            return new List<Transaction>();
        }
        private async Task<IEnumerable<Product>> ReadProductsAsync(string productFilePath)
        {
            if (!string.IsNullOrWhiteSpace(productFilePath))
            {
                using (var package = new ExcelPackage(new FileInfo(productFilePath)))
                {
                    List<Product> records = new List<Product>();
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    List<Task> tasks = new List<Task>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var currentRow = row;
                        tasks.Add(Task.Run(() =>
                        {
                            var code = int.Parse(worksheet.Cells[currentRow, 1].Text);
                            var count = decimal.Parse(worksheet.Cells[currentRow, 3].Text, CultureInfo.InvariantCulture.NumberFormat);
                            var unit = worksheet.Cells[currentRow, 4].Text.Trim() == "كيلوگرم" ? UnitType.Kilogram
                                : worksheet.Cells[currentRow, 4].Text.Trim() == "عدد" ? UnitType.Num : UnitType.Unknow;
                            var price = !string.IsNullOrWhiteSpace(worksheet?.Cells[currentRow, 5].Text) ? int.Parse(worksheet?.Cells[currentRow, 5].Text?.Replace(",", "")?.ToString() ?? "0") : 0;
                            var product = new Product
                            {
                                Code = code,
                                Quantity = Convert.ToInt32(count),
                                Price = price,
                                UnitType = unit
                            };
                            lock (records) { records.Add(product); }
                        }));
                    }
                    await Task.WhenAll(tasks);
                    return records;
                }
            };
            return new List<Product>();
        }
        private void CreteInvoiceAsync(string outputPath, List<Transaction> transactions, List<Product> products, int invoiceNumberIndex, string dateStart)
        {
            List<Invoice> invoices = new List<Invoice>();
            var masterProducts = products.Where(e => e.Price > 0).ToList();
            var cheapProducts = products.Any(e => e.Price == 0) ? products.Where(e => e.Price == 0).ToList() : FakeProducts();
            long minimumPrice = masterProducts.OrderBy(s => s.Price).Select(s => s.Price).FirstOrDefault();
            var averagePrice = (int)masterProducts.Average(e => e.Price);

            int pageSize = 10;
            var TotalPages = (int)Math.Ceiling(transactions.Count / (double)pageSize);
            var invoiceNumber = invoiceNumberIndex;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = TotalPages;
            progressBar1.Step = 1;

            for (int partition = 0; partition < TotalPages; partition++)
            {
                var filteredPrices = transactions.Skip(partition * pageSize).Take(pageSize).ToList();
                List<Task> tasks = new List<Task>();
                for (int i = 0; i < filteredPrices.Count(); i++)
                {
                    var transaction = filteredPrices[i];
                    tasks.Add(Task.Run(() => PriceProccess(transaction, invoiceNumber++, minimumPrice, averagePrice, ref masterProducts, ref cheapProducts, invoices)));
                    Task.WaitAll(tasks.ToArray());
                    tasks.Clear();
                }
                progressBar1.PerformStep();
            }

            GenerateExcelInvoice(invoices, outputPath, dateStart);

            txtCheap.Text = invoices.Where(el => cheapProducts.Select(e => e.Code).Contains(el.ProductCode))
                .GroupBy(e => e.ProductCode)
                .Select(s => new
                {
                    Code = s.Key,
                    Total = s.Sum(e => e.Quantity)
                }).Select(el => string.Join(",","Code"+el.Code +" => Count : "+ el.Total)).ToList().Aggregate((a, b) => a + "  ,  " + b);

            MessageBox.Show($"Invoice file created successfully! \n in path : {outputPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        static void PriceProccess(Transaction transaction, int invoiceNumber, long minimumProductPrice, int averagePrice, ref List<Product> masterProducts, ref List<Product> cheapProducts, List<Invoice> invoices)
        {

            var random = new Random();
            var selectedProducts = new List<Product>();
            long totalAmount = 0;
            long regulator = 0;
            long remainingPrice = 0;
              
            while (totalAmount < transaction.Amount)
            {
                masterProducts = masterProducts.Where(el => el.Quantity > 0).ToList();
                masterProducts = masterProducts?.Any() == true ? masterProducts : cheapProducts;
                var filterdProducts = regulator > 0 ? masterProducts.Where(e => e.Price <= regulator).ToList() : masterProducts.Where(e => e.Price >= averagePrice).ToList();
                filterdProducts = filterdProducts?.Any() == true ? filterdProducts : cheapProducts;
                var product = filterdProducts[random.Next(filterdProducts.Count)];

                int calculatedQuantity = 0;
                if (totalAmount > 0)
                {
                    remainingPrice = transaction.Amount - selectedProducts.Sum(e => e.Quantity * e.Price);
                    calculatedQuantity = remainingPrice <= product.Price || remainingPrice == 0 || product.Price == 0 ? 1 : (int)Math.Floor((decimal)(remainingPrice / product.Price));
                }
                else
                    calculatedQuantity = (int)(transaction.Amount > product.Price && product.Price > 0 ? Math.Floor((decimal)(transaction.Amount / product.Price)) : product.Quantity);

                calculatedQuantity = calculatedQuantity >= product.Quantity ? product.Quantity > 10 ? 10 : product.Quantity : calculatedQuantity;
                int quantity = random.Next(1, calculatedQuantity);

                long totalProductAmount = quantity * product.Price;
                if (totalAmount + totalProductAmount <= transaction.Amount && product.Price > 0)
                {
                    masterProducts.Where(e => e.Code == product.Code).Select(s => s.Quantity -= quantity).ToList();
                    if (selectedProducts.Any(e => e.Code == product.Code))
                        selectedProducts.Where(e => e.Code == product.Code).Select(s => s.Quantity += quantity).ToList();
                    else
                        selectedProducts.Add(new Product { Code = product.Code, Price = product.Price, Quantity = quantity, UnitType = product.UnitType });
                    totalAmount += totalProductAmount;
                }
                if (totalAmount == transaction.Amount) break;

                if ((transaction.Amount - totalAmount) < minimumProductPrice || product.Price == 0)
                {
                    decimal remainedPrice = transaction.Amount - totalAmount;
                    if (remainedPrice >= 10000)
                    {
                        decimal calc = remainedPrice / 10000;
                        var firstCheap = cheapProducts.FirstOrDefault();
                        selectedProducts.Add(new Product { Code = firstCheap.Code, Price = 10000, Quantity = Convert.ToInt32(Math.Floor(calc)), UnitType = firstCheap.UnitType });                     
                        if (calc % 2 > 0)
                        {
                            var chunckedPrice = int.Parse((calc % 2).ToString("#.0000").Split('.')[1]);
                            if (chunckedPrice == 0) break;
                            var chuckedProduct = cheapProducts.Where(e => e.Code != firstCheap.Code).ToList()[random.Next(cheapProducts.Count - 1)];
                            selectedProducts.Add(new Product { Code = chuckedProduct.Code, Price = chunckedPrice, Quantity = 1, UnitType = chuckedProduct.UnitType });                           
                        }
                    }
                    else
                    {
                        var firstCheap = cheapProducts[random.Next(cheapProducts.Count)];
                        selectedProducts.Add(new Product { Code = firstCheap.Code, Price = (long)remainedPrice, Quantity = 1, UnitType = firstCheap.UnitType });                      
                    }
                    break;
                }
                else if ((totalAmount + totalProductAmount) > transaction.Amount)
                {
                    regulator = transaction.Amount - totalAmount;
                }
                else { regulator = totalAmount > 0 ? transaction.Amount > totalAmount ? transaction.Amount - totalAmount : totalAmount - transaction.Amount : 0; }
            }

            foreach (var item in selectedProducts)
            {
                invoices.Add(new Invoice
                {
                    Number = invoiceNumber,
                    ProductCode = item.Code,
                    Quantity = item.Quantity,
                    Total = item.Price,
                    UnitType = item.UnitType,
                    PrimaryPrice = transaction.Amount,
                    TransactionDescription = transaction.Description
                });
            }
        }
        private List<Product> FakeProducts() => new List<Product>
        {
            new Product
            {
                Code = 100001,
                Price = 0,
                Quantity = 0,
                UnitType = UnitType.Num
            },
            new Product
            {
                Code = 100002,
                Price = 0,
                Quantity = 0,
                UnitType = UnitType.Num
            },
        };
        private void GenerateExcelInvoice(List<Invoice> invoices, string outputPath, string startDate)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Invoices");
                var headers = invoiceHeaders;
                for (int i = 0; i < headers.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                }

                SetStyle(worksheet, headers.Count());
                int row = 2;

                var orderedinvoices = radioAsc.Checked ? invoices.OrderBy(e => radioInvoice.Checked ? e.Number : e.PrimaryPrice)
                    .ThenByDescending(e => e.Total).ToList()
                    : invoices.OrderByDescending(e => radioInvoice.Checked ? e.Number : e.PrimaryPrice)
                    .ThenByDescending(e => e.Total).ToList();

                for (int i = 0; i < orderedinvoices.Count(); i++)
                {
                    worksheet.Cells[row, 1].Value = "InvoiceItem";
                    worksheet.Cells[row, 2].Value = orderedinvoices[i].Number;
                    worksheet.Cells[row, 3].Value = startDate;
                    worksheet.Cells[row, 4].Value = $"101496";
                    worksheet.Cells[row, 5].Value = $"8";
                    worksheet.Cells[row, 6].Value = orderedinvoices[i].ProductCode;
                    worksheet.Cells[row, 7].Value = $"1";
                    worksheet.Cells[row, 8].Value = $"";
                    worksheet.Cells[row, 9].Value = orderedinvoices[i].UnitType == UnitType.Kilogram ? "کیلوگرم" : orderedinvoices[i].UnitType == UnitType.Num ? "عدد" : "";
                    worksheet.Cells[row, 10].Value = "";
                    worksheet.Cells[row, 11].Value = orderedinvoices[i].Total; //قلم فی
                    worksheet.Cells[row, 12].Value = orderedinvoices[i].Quantity; //قلم فاکتور کل
                    worksheet.Cells[row, 13].Value = 0;
                    worksheet.Cells[row, 14].Value = 0;
                    worksheet.Cells[row, 15].Value = 0;
                    worksheet.Cells[row, 16].Value = 0;
                    worksheet.Cells[row, 17].Value = orderedinvoices[i].TransactionDescription;
                    worksheet.Cells[row, 18].Value = $"مشتري متفرقه-مصرف كننده نهايي";
                    worksheet.Cells[row, 19].Value = $"آدرس مشتري";
                    worksheet.Cells[row, 20].Value = 0;
                    worksheet.Cells[row, 21].Value = 0;
                    worksheet.Cells[row, 22].Value = $"ريال";
                    worksheet.Cells[row, 23].Value = 1;
                    worksheet.Cells[row, 24].Value = 1;
                    row++;
                }
                package.SaveAs(new FileInfo(outputPath));
            }
        }
        private void SetStyle(ExcelWorksheet worksheet, int headerCount)
        {
            for (int col = 1; col <= headerCount; col++) { worksheet.Column(col).Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.DarkGray); }
            worksheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Column(1).Width = 13;
            worksheet.Column(2).Width = 12;
            worksheet.Column(2).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Column(2).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

            worksheet.Column(3).Width = 14;
            worksheet.Column(3).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Column(3).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

            worksheet.Column(4).Width = 14;
            worksheet.Column(5).Width = 15;
            worksheet.Column(6).Width = 13;
            worksheet.Column(6).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Column(6).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

            worksheet.Column(7).Width = 15;
            worksheet.Column(8).Width = 18;
            worksheet.Column(9).Width = 17;
            worksheet.Column(9).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Column(9).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

            worksheet.Column(10).Width = 19;
            worksheet.Column(11).Width = 15;
            worksheet.Column(11).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Column(11).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

            worksheet.Column(12).Width = 12;
            worksheet.Column(12).Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Column(12).Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

            worksheet.Column(13).Width = 14;
            worksheet.Column(14).Width = 15;
            worksheet.Column(15).Width = 34;
            worksheet.Column(16).Width = 16;
            worksheet.Column(17).Width = 16;
            worksheet.Column(18).Width = 34;
            worksheet.Column(19).Width = 16;
            worksheet.Column(20).Width = 23;
            worksheet.Column(21).Width = 35;
            worksheet.Column(22).Width = 12;
            worksheet.Column(23).Width = 15;
            worksheet.Column(24).Width = 15;
        }
        private class Product
        {
            public int Code { get; set; }
            public int Quantity { get; set; }
            public UnitType UnitType { get; set; }
            public long Price { get; set; }
        }
        private struct Invoice
        {
            public int Number { get; set; }
            public int ProductCode { get; set; }
            public int Quantity { get; set; }
            public decimal Total { get; set; }
            public UnitType UnitType { get; set; }
            public decimal PrimaryPrice { get; set; }
            public string TransactionDescription { get; set; }
        }
        public class Transaction
        {
            public long Amount { get; set; }
            public string Description { get; set; }
        }
        private enum UnitType
        {
            Kilogram = 1,
            Num = 2,
            Unknow = 3
        }

        private List<string> invoiceHeaders = new List<string>
        {
            "نوع قلم",
            "فاكتور شماره",
            "فاكتور تاريخ",
            "فاكتور كد مشتري",
            "فاكتور كد نوع فروش",
            "قلم فاكتور كد",
            "قلم فاكتور كد انبار",
            "قلم فاكتور عنوان رديابي",
            "قلم فاكتور واحد اصلي",
            "قلم فاكتور واحد فرعي1",
            "قلم فاكتور في",
            "قلم فاكتور كل",
            "قلم فاكتور ماليات",
            "قلم فاكتور عوارض",
            "قلم فاكتور تخفيف مبلغي اعلاميه قيمت",
            "قلم فاكتور اضافات",
            "قلم فاكتور توضيحات",
            "فاكتور نام مشتري",
            "فاكتور محل تحويل",
            "قلم فاكتور تخفيف مشتري",
            "قلم فاكتور تخفيف درصدي اعلاميه قيمت",
            "فاكتور ارز1",
            "فاكتور نرخ ارز",
            "فاكتور نوع تسویه",
        };

        private void Form1_Load(object sender, EventArgs e)
        {
            txtDate.Text = "1403/07/10";
            txtInvoiceNumber.Text = "1000";
            radioInvoice.Checked = true;
            radioAsc.Checked = true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FileDialog fileDialog = new OpenFileDialog())
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtTransaction.Text = openFileDialog1.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FileDialog fileDialog = new OpenFileDialog())
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtProduct.Text = openFileDialog1.FileName;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtSavePath.Text = folderBrowserDialog1.SelectedPath;

            }
        }
    }
}
