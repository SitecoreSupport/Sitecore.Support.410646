namespace Sitecore.Support.Commerce.Engine.Connect.Search.ComputedFields
{
    using Sitecore.Commerce.Engine.Connect;
    using Sitecore.Commerce.Engine.Connect.Search;
    using Sitecore.ContentSearch;
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using System.Collections.Generic;

    public class CommerceSearchItemTypeField : BaseCommerceComputedField
    {
        private static readonly IEnumerable<ID> _validTemplates = new List<ID>
        {
            CommerceConstants.KnownTemplateIds.CommerceProductTemplate,
            CommerceConstants.KnownTemplateIds.CommerceProductVariantTemplate,
            CommerceConstants.KnownTemplateIds.CommerceBundleTemplate,
            CommerceConstants.KnownTemplateIds.CommerceCategoryTemplate,
            CommerceConstants.KnownTemplateIds.CommerceNavigationItemTemplate,
            CommerceConstants.KnownTemplateIds.CommerceCatalogTemplate
        };

        protected override IEnumerable<ID> ValidTemplates
        {
            get
            {
                return _validTemplates;
            }
        }

        public override object ComputeValue(IIndexable indexable)
        {
            Assert.ArgumentNotNull(indexable, nameof(indexable));
            var returnType = CommerceSearchItemType.Unknown;

            var validatedItem = GetValidatedItem(indexable);
            if (validatedItem != null)
            {
                returnType = this.GetItemType(validatedItem);
            }

            return returnType;
        }
    }
}
