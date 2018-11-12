//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v3.0.10.102
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace Setas.Core.Models
{
	/// <summary>Mushroom</summary>
	[PublishedContentModel("Mushroom")]
	public partial class Mushroom : PublishedContentModel
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "Mushroom";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Mushroom(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Mushroom, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Class
		///</summary>
		[ImplementPropertyType("class")]
		public IEnumerable<string> Class
		{
			get { return this.GetPropertyValue<IEnumerable<string>>("class"); }
		}

		///<summary>
		/// Confusion
		///</summary>
		[ImplementPropertyType("confusion")]
		public IHtmlString Confusion
		{
			get { return this.GetPropertyValue<IHtmlString>("confusion"); }
		}

		///<summary>
		/// Cooking Instructions
		///</summary>
		[ImplementPropertyType("cookingInstructions")]
		public IHtmlString CookingInstructions
		{
			get { return this.GetPropertyValue<IHtmlString>("cookingInstructions"); }
		}

		///<summary>
		/// Cooking Interest
		///</summary>
		[ImplementPropertyType("cookingInterest")]
		public IEnumerable<IPublishedContent> CookingInterest
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("cookingInterest"); }
		}

		///<summary>
		/// Description
		///</summary>
		[ImplementPropertyType("description")]
		public IHtmlString Description
		{
			get { return this.GetPropertyValue<IHtmlString>("description"); }
		}

		///<summary>
		/// Family
		///</summary>
		[ImplementPropertyType("family")]
		public IEnumerable<string> Family
		{
			get { return this.GetPropertyValue<IEnumerable<string>>("family"); }
		}

		///<summary>
		/// Habitat
		///</summary>
		[ImplementPropertyType("habitat")]
		public IHtmlString Habitat
		{
			get { return this.GetPropertyValue<IHtmlString>("habitat"); }
		}

		///<summary>
		/// Images
		///</summary>
		[ImplementPropertyType("images")]
		public IEnumerable<IPublishedContent> Images
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("images"); }
		}

		///<summary>
		/// Order
		///</summary>
		[ImplementPropertyType("order")]
		public IEnumerable<string> Order
		{
			get { return this.GetPropertyValue<IEnumerable<string>>("order"); }
		}

		///<summary>
		/// Popular Names
		///</summary>
		[ImplementPropertyType("popularNames")]
		public IEnumerable<string> PopularNames
		{
			get { return this.GetPropertyValue<IEnumerable<string>>("popularNames"); }
		}

		///<summary>
		/// Season
		///</summary>
		[ImplementPropertyType("season")]
		public IHtmlString Season
		{
			get { return this.GetPropertyValue<IHtmlString>("season"); }
		}

		///<summary>
		/// Subclass
		///</summary>
		[ImplementPropertyType("subclass")]
		public IEnumerable<string> Subclass
		{
			get { return this.GetPropertyValue<IEnumerable<string>>("subclass"); }
		}

		///<summary>
		/// Synonyms
		///</summary>
		[ImplementPropertyType("synonyms")]
		public IEnumerable<string> Synonyms
		{
			get { return this.GetPropertyValue<IEnumerable<string>>("synonyms"); }
		}
	}
}
