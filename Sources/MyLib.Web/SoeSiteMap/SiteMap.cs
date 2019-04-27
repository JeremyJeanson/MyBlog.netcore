using MyLib.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MyLib.Web.SoeSiteMap
{
    public sealed class SoeSiteMap
    {
        #region Déclarations

        private const String XmlNamespaceuri = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private static readonly XNamespace XmlNamespace;
        private readonly SoeSiteMapItem[] _items;

        #endregion

        #region Constructeurs

        static SoeSiteMap()
        {
            XmlNamespace = XmlNamespaceuri;
        }


        public SoeSiteMap(SoeSiteMapItem[] items)
        {
            _items = items;
        }

        #endregion

        #region Propriétés

        #endregion

        #region Méthodes

        /// <summary>
        /// Get XML for this sitemap
        /// </summary>
        /// <returns></returns>
        internal XDocument GetXDocument()
        {
            return new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(XmlNamespace + "urlset", GetElements()));
        }

        private IEnumerable<XObject> GetElements()
        {
            yield return new XAttribute("xmlns", XmlNamespace);
            foreach (var item in _items)
            {
                yield return GetNode(item);
            }
        }

        /// <summary>
        /// Get XML Node for this item
        /// </summary>
        /// <returns></returns>
        internal static XElement GetNode(SoeSiteMapItem item)
        {
            // Create an XML node
            XElement result = new XElement(XmlNamespace + "url",
                new XElement(XmlNamespace + "loc", item.Uri));

            // Add Lastmodified
            if (item.Lastmodified.HasValue)
            {
                result.Add(new XElement(XmlNamespace + "lastmod", DateHelper.GetW3CFormat(item.Lastmodified.Value)));
            }

            // ChangeFrequence
            if (item.ChangeFrequence.HasValue)
            {
                result.Add(new XElement(XmlNamespace + "changefreq", GetFrequence(item.ChangeFrequence.Value)));
            }

            // Priority
            if (item.Priority.HasValue)
            {
                result.Add(new XElement(XmlNamespace + "priority", item.Priority.Value.ToString()));
            }

            // Return the XML node
            return result;
        }

        /// <summary>
        /// Foramt frequence
        /// </summary>
        /// <param name="frequence"></param>
        /// <returns></returns>
        private static String GetFrequence(Frequence frequence)
        {
            switch (frequence)
            {
                case Frequence.Always:
                    {
                        return "always";
                    }
                case Frequence.Daily:
                    {
                        return "daily";
                    }
                case Frequence.Hourly:
                    {
                        return "hourly";
                    }
                case Frequence.Monthly:
                    {
                        return "monthly";
                    }
                case Frequence.Never:
                    {
                        return "never";
                    }
                case Frequence.Weekly:
                    {
                        return "weekly";
                    }
                case Frequence.Yearly:
                    {
                        return "yearly";
                    }
                default: { return "always"; }
            }
        }

        #endregion
    }
}
