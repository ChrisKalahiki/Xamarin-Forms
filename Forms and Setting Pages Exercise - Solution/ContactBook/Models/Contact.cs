using System;
using System.ComponentModel;

namespace ContactBook
{
	public class Contact 
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool IsBlocked { get; set; }

		public string FullName
		{
			// Note the string interpolation syntax in C# 6. Read my blog post
			// for details: 
			// 
			// http://programmingwithmosh.com/csharp/csharp-6-features-that-help-you-write-cleaner-code/
			get { return $"{FirstName} {LastName}"; }
		}
	}
}
