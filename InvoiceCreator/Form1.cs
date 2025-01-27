using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

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
                                    decimal sumTransactions = transactionAmounts.Sum(e => e.Amount);
                                    if (sumTransactions >= totalProduct)
                                        MessageBox.Show("جمع کل موجودی کالا از جمع کل مجموع تراکنش بیشتر است");
                                    else
                                        CreteInvoiceAsync(invoiceFilePath, transactionAmounts.ToList(), products.ToList(), invoiceNumberIndex, txtDate.Text, radioModeB.Checked, twoDecimalPointRadio.Checked);
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
                int index = 1;
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
                    var transaction = records.OrderByDescending(e => e.Amount).OrderBy(e => e.Description).ToList();
                    transaction.ForEach(e => { e.Index = index++; });
                    return transaction;
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
                    return records.OrderByDescending(e => e.Quantity).ToList();
                }
            };
            return new List<Product>();
        }
        private void CreteInvoiceAsync(string outputPath, List<Transaction> transactions, List<Product> products, int invoiceNumberIndex, string dateStart, bool chunkMode, bool twoDecimalPoint)
        {
            List<Invoice> invoices = new List<Invoice>();
            var indexPrimary = 1;
            var masterProducts = products.Where(e => e.Price > 0).ToList();
            masterProducts.ForEach(el =>
            {
                if (indexPrimary <= 5)
                {
                    el.IsPrimary = true;
                    indexPrimary++;
                }
            });
            var cheapProducts = products.Any(e => e.Price == 0) ? GeneratePriceForCheatProducts(products.Where(e => e.Price == 0).ToList()) : FakeProducts();
            decimal minimumPrice = masterProducts.OrderBy(s => s.Price).Select(s => s.Price).FirstOrDefault();
            decimal averagePrice = (int)masterProducts.Average(e => e.Price);

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
                    tasks.Add(Task.Run(() => PriceProccess(transaction, invoiceNumber++, minimumPrice, averagePrice, ref masterProducts, ref cheapProducts, invoices, chunkMode, twoDecimalPoint)));
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
                    Total = (int)s.Sum(e => e.Quantity)
                }).Select(el => string.Join(",", "Code" + el.Code + " => Count : " + el.Total)).ToList().Aggregate((a, b) => a + "  ,  " + b);

            MessageBox.Show($"Invoice file created successfully! \n in path : {outputPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        static void PriceProccess(Transaction transaction, int invoiceNumber, decimal minimumProductPrice, decimal averagePrice, ref List<Product> masterProducts, ref List<Product> cheapProducts, List<Invoice> invoices, bool chunkMode, bool twoDecimalPoint)
        {
            var selectedProducts = new List<Product>();
            decimal totalAmount = 0;
            decimal regulator = 0;
            decimal remainingPrice = 0;

            while (totalAmount < transaction.Amount)
            {
                masterProducts = masterProducts.Where(el => el.Quantity > 0).ToList();
                cheapProducts = cheapProducts.Where(e => e.Quantity > 0).ToList();
                masterProducts = masterProducts?.Any() == true ? masterProducts : cheapProducts;
                var filterdProducts = regulator > 0 ? masterProducts.Where(e => e.Price <= regulator).OrderByDescending(e => e.Price).ToList() : masterProducts;
                filterdProducts = filterdProducts?.Any() == true ? filterdProducts : masterProducts.ToList();

                if (CheckProductPriceEqualityWithTransaction(transaction.Amount, ref filterdProducts, ref selectedProducts, ref selectedProducts)) break;
                var ProductAndQuantity = InitializeSelectProduct(transaction.Amount, ref masterProducts, ref filterdProducts, ref selectedProducts, ref remainingPrice, ref totalAmount, twoDecimalPoint);
                if (totalAmount == transaction.Amount) break;

                if ((transaction.Amount - totalAmount) < minimumProductPrice || ProductAndQuantity.product.Price == 0)
                {
                    Rounding(transaction.Amount, ref masterProducts, ref filterdProducts, ref selectedProducts, ref remainingPrice, ref totalAmount, ref cheapProducts, chunkMode);
                    break;
                }
                else if ((totalAmount + (ProductAndQuantity.product.Price * ProductAndQuantity.quantity)) > transaction.Amount)
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
                    Quantity = Math.Round(item.Quantity, 4),
                    Total = item.Price,
                    UnitType = item.UnitType,
                    PrimaryPrice = transaction.Amount,
                    TransactionDescription = transaction.Description,
                    TransactionIndex = transaction.Index
                });
            }
        }
        static (Product product, decimal quantity) InitializeSelectProduct(decimal transactionPrice, ref List<Product> masterProducts, ref List<Product> filteredProducts, ref List<Product> selectedProducts, ref decimal remainingPrice, ref decimal totalAmount, bool twoDecimalPoint)
        {
            var random = new Random();
            Product? product = new Product();
            decimal quantity = 1;
            decimal calculatedQuantity = 0;
            if (selectedProducts?.Any() == false && filteredProducts.Any(e => e.IsPrimary && e.Quantity >= 1))
            {
                var primaryProducts = filteredProducts.Where(e => e.IsPrimary && e.Price <= transactionPrice).ToList();
                if (primaryProducts?.Any() == true)
                {
                    var minimumPrimaryPrice = primaryProducts.OrderBy(e => e.Price).FirstOrDefault();
                    if ((transactionPrice <= minimumPrimaryPrice.Price) || (transactionPrice <= (minimumPrimaryPrice.Price * 2)))
                    {
                        product = minimumPrimaryPrice;
                        selectedProducts.Add(new Product { Code = minimumPrimaryPrice.Code, Price = minimumPrimaryPrice.Price, Quantity = 1, UnitType = product.UnitType });
                        masterProducts.Where(e => e.Code == product.Code).Select(s => s.Quantity -= 1).ToList();
                        remainingPrice = transactionPrice - (1 * product.Price);
                        totalAmount = 1 * product.Price;
                    }
                    else
                    {
                        if (primaryProducts.Any(el => el.UnitType == UnitType.Kilogram))
                        {
                            var kilogramProduct = primaryProducts.Where(e => e.UnitType == UnitType.Kilogram).ToList();
                            product = kilogramProduct[random.Next(kilogramProduct.Count)];
                            decimal totalUnit = transactionPrice / product.Price;
                            if (product.Quantity >= totalUnit)
                            {
                                int integerPart = (int)totalUnit;
                                decimal fractionalPart = totalUnit - integerPart;
                                decimal adjustedFractionalPart = Math.Floor(fractionalPart * (twoDecimalPoint ? 100 : 10)) / (twoDecimalPoint ? 100 : 10);
                                var calculatedKiloQuantity = decimal.Parse((integerPart + adjustedFractionalPart).ToString("F3"));
                                quantity = calculatedKiloQuantity;
                                selectedProducts.Add(new Product { Code = product.Code, Price = product.Price, Quantity = quantity, UnitType = product.UnitType });
                                masterProducts.Where(e => e.Code == product.Code).Select(s => s.Quantity -= quantity).ToList();
                                totalAmount = quantity * product.Price;
                                remainingPrice = transactionPrice - (quantity * product.Price);
                            }
                            else
                                product = null;
                        }
                        else
                        {
                            product = primaryProducts[random.Next(primaryProducts.Count())];
                            if (transactionPrice >= product.Price)
                            {
                                quantity = (int)(Math.Floor(transactionPrice / product.Price));
                                if (product.Quantity >= quantity)
                                {
                                    selectedProducts.Add(new Product { Code = product.Code, Price = product.Price, Quantity = quantity, UnitType = product.UnitType });
                                    masterProducts.Where(e => e.Code == product.Code).Select(s => s.Quantity -= quantity).ToList();
                                    totalAmount = quantity * product.Price;
                                    remainingPrice = transactionPrice - (quantity * product.Price);
                                }
                                else
                                    product = null;
                            }
                            else
                                product = null;
                        }
                    }
                }
            }
            if (product is null || (product?.Price == 0 && product?.Quantity == 0))
            {
                decimal calculatedPrice = remainingPrice > 0 ? remainingPrice : transactionPrice;
                filteredProducts = masterProducts.Where(e => e.Price <= calculatedPrice).OrderByDescending(e => e.Price).Take(1).ToList();
                if (filteredProducts?.Any() == true)
                {
                    product = filteredProducts[random.Next(filteredProducts.Count)];
                    if (totalAmount > 0)
                    {
                        remainingPrice = transactionPrice - selectedProducts.Sum(e => e.Quantity * e.Price);
                        calculatedQuantity = remainingPrice <= product.Price || remainingPrice == 0 || product.Price == 0 ? 1 : (int)Math.Floor((decimal)(remainingPrice / product.Price));
                    }
                    else
                        calculatedQuantity = (int)(transactionPrice > product.Price && product.Price > 0 ? Math.Floor((decimal)(transactionPrice / product.Price)) : product.Quantity);

                    calculatedQuantity = calculatedQuantity >= product.Quantity ? product.Quantity > 10 ? 10 : product.Quantity : calculatedQuantity;
                    quantity = random.Next(1, (int)calculatedQuantity);

                    decimal totalProductAmount = quantity * product.Price;
                    if (totalAmount + totalProductAmount <= transactionPrice && product.Price > 0)
                    {
                        masterProducts.Where(e => e.Code == product.Code).Select(s => s.Quantity -= quantity).ToList();
                        if (selectedProducts.Any(e => e.Code == product.Code))
                            selectedProducts.Where(e => e.Code == product.Code).Select(s => s.Quantity += quantity).ToList();
                        else
                            selectedProducts.Add(new Product { Code = product.Code, Price = product.Price, Quantity = quantity, UnitType = product.UnitType });
                        totalAmount += totalProductAmount;
                    }
                }
            }
            return (product, quantity);
        }
        static void Rounding(decimal transactionPrice, ref List<Product> masterProducts, ref List<Product> filteredProducts, ref List<Product> selectedProducts, ref decimal remainingPrice, ref decimal totalAmount, ref List<Product> cheapProducts, bool chunkMode)
        {
            decimal remainedPrice = transactionPrice - totalAmount;

            var random = new Random();
            if (remainedPrice >= 10000)
            {
                var calc = Convert.ToInt32(remainedPrice / 10000);
                var firstCheap = cheapProducts.OrderByDescending(e => e.Quantity).Where(e => e.Quantity > 0 && e.Price == 10000).FirstOrDefault();
                if (selectedProducts.Any(e => e.Code == firstCheap.Code))
                {
                    selectedProducts.Where(e => e.Code == firstCheap.Code).Select(s => (s.Quantity = chunkMode ? 1 : (s.Quantity + calc))).ToList();
                    selectedProducts.Where(e => e.Code == firstCheap.Code).Select(s => (s.Price = chunkMode ? (s.Price + (calc * firstCheap.Price)) : firstCheap.Price)).ToList();
                }
                else
                {
                    selectedProducts.Add(new Product { Code = firstCheap.Code, Price = chunkMode ? (calc * firstCheap.Price) : firstCheap.Price, Quantity = chunkMode ? 1 : calc, UnitType = firstCheap.UnitType });
                }
            }
            else if (remainedPrice >= 1000)
            {
                var calc = Convert.ToInt32(remainedPrice / 1000);
                var firstCheap = cheapProducts.OrderByDescending(e => e.Quantity).Where(e => e.Quantity > 0 && e.Price == 1000).FirstOrDefault();
                selectedProducts.Add(new Product { Code = firstCheap.Code, Price = chunkMode ? (calc * firstCheap.Price) : firstCheap.Price, Quantity = chunkMode ? 1 : calc, UnitType = firstCheap.UnitType });
            }
            else
            {
                var calc = Convert.ToInt32(remainedPrice / 100);
                var firstCheap = cheapProducts.OrderByDescending(e => e.Quantity).Where(e => e.Quantity > 0 && e.Price == 100).FirstOrDefault();
                selectedProducts.Add(new Product { Code = firstCheap.Code, Price = chunkMode ? (calc * firstCheap.Price) : firstCheap.Price, Quantity = chunkMode ? 1 : calc, UnitType = firstCheap.UnitType });
            }

        }
        static bool CheckProductPriceEqualityWithTransaction(decimal transactionPrice, ref List<Product> masterProducts, ref List<Product> filteredProducts, ref List<Product> selectedProducts)
        {
            if (filteredProducts.Any(e => e.Price == transactionPrice))
            {
                var findProduct = filteredProducts.Where(e => e.Price == transactionPrice).FirstOrDefault();
                selectedProducts.Add(new Product { Code = findProduct.Code, Price = findProduct.Price, Quantity = 1, UnitType = findProduct.UnitType });
                masterProducts.Where(e => e.Code == findProduct.Code).Select(s => s.Quantity -= 1).ToList();
                return true;
            }
            else if (filteredProducts.Any(e => (transactionPrice / 2) == e.Price))
            {
                var findProduct = filteredProducts.Where(e => (transactionPrice / 2) == e.Price).FirstOrDefault();
                selectedProducts.Add(new Product { Code = findProduct.Code, Price = findProduct.Price, Quantity = 2, UnitType = findProduct.UnitType });
                masterProducts.Where(e => e.Code == findProduct.Code).Select(s => s.Quantity -= 2).ToList();
                return true;
            }
            return false;
        }
        private List<Product> GeneratePriceForCheatProducts(List<Product> cheatProducts)
        {
            int index = 1;
            cheatProducts.ForEach(el =>
            {
                el.Price = index % 2 == 0 ? 10000 : 1000;
                ++index;
            });
            return cheatProducts;
        }
        private List<Product> FakeProducts() => new List<Product>
        {
            new Product
            {
                Code = 100001,
                Price = 10000,
                Quantity = 7000,
                UnitType = UnitType.Num
            },
            new Product
            {
                Code = 100002,
                Price = 1000,
                Quantity = 7000,
                UnitType = UnitType.Num
            },
            new Product
            {
                Code = 100003,
                Price = 10000,
                Quantity = 7000,
                UnitType = UnitType.Num
            },
            new Product
            {
                Code = 100004,
                Price = 10000,
                Quantity = 7000,
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
                    worksheet.Cells[row, 12].Value = orderedinvoices[i].Quantity.ToString(); //قلم فاکتور کل
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
            public decimal Quantity { get; set; }
            public UnitType UnitType { get; set; }
            public decimal Price { get; set; }
            public bool IsPrimary { get; set; }
        }
        private struct Invoice
        {
            public int Number { get; set; }
            public int ProductCode { get; set; }
            public decimal Quantity { get; set; }
            public decimal Total { get; set; }
            public UnitType UnitType { get; set; }
            public decimal PrimaryPrice { get; set; }
            public string TransactionDescription { get; set; }
            public int TransactionIndex { get; set; }
        }
        public class Transaction
        {
            public decimal Amount { get; set; }
            public string Description { get; set; }
            public int Index { get; set; }
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
