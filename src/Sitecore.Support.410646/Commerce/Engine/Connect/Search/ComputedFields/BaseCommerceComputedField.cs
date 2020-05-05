namespace Sitecore.Support.Commerce.Engine.Connect.Search.ComputedFields
{
    using ContentSearch;
    using ContentSearch.ComputedFields;
    using Data.Managers;
    using Diagnostics;
    using Sitecore.Commerce.Engine.Connect;
    using Sitecore.Commerce.Engine.Connect.Interfaces;
    using Sitecore.Commerce.Engine.Connect.Search;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using System.Collections.Generic;

    public abstract class BaseCommerceComputedField : IComputedIndexField
    {
        #region IComputedIndexField
        public string FieldName
        {
            get;
            set;
        }

        public string ReturnType
        {
            get;
            set;
        }

        #endregion

        #region Abstract properties

        protected abstract IEnumerable<ID> ValidTemplates
        {
            get;
        }

        #endregion

        #region Static helper methods

        protected virtual string GetItemType(Item item)
        {
            var returnType = CommerceSearchItemType.Unknown;

            var searchManager = CommerceTypeLoader.CreateInstance<ICommerceSearchManager>();
            if (searchManager.IsItemProduct(item))
            {
                returnType = CommerceSearchItemType.SellableItem;
            }
            else if (searchManager.IsItemCategory(item))
            {
                returnType = CommerceSearchItemType.Category;
            }
            else if (searchManager.IsItemCatalog(item))
            {
                returnType = CommerceSearchItemType.Catalog;
            }
            else if (searchManager.IsItemNavigation(item))
            {
                returnType = CommerceSearchItemType.Navigation;
            }

            return returnType;
        }

        #endregion

        public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
        {
            return this.ComputeValue(indexable);
        }

        #region Abstract methods

        public abstract object ComputeValue(Sitecore.ContentSearch.IIndexable itemToIndex);
        #endregion

        #region Helper methods

        #region modified part

        public bool IsItemPartOfValidTemplates(Item item)
        {
            Assert.IsNotNull(item, nameof(item));

            foreach (var templateID in this.ValidTemplates)
            {
                if (TemplateManager.GetTemplate(item).DescendsFromOrEquals(templateID))
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        public Item GetValidatedItem(IIndexable itemToIndex)
        {
            var siteCoreItemToIndex = itemToIndex as SitecoreIndexableItem;

            if (siteCoreItemToIndex == null)
            {
                return null;
            }

            var item = siteCoreItemToIndex.Item;
            if (!this.IsItemPartOfValidTemplates(item) || item.Name == Sitecore.Constants.StandardValuesItemName)
            {
                return null;
            }

            return item;
        }

        #endregion
    }
}