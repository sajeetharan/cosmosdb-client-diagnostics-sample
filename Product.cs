﻿using Newtonsoft.Json;

namespace ClientDiagnostics
{
	public class Product
	{
		public string id { get; set; }
		public string categoryId { get; set; }
		public string categoryName { get; set; }
		public string sku { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public double price { get; set; }
	}
}
