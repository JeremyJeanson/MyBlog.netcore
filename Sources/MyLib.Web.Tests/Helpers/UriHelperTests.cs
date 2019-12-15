using MyLib.Web.Helpers;
using System;
using Xunit;

namespace MyLib.Web.Tests.Helpers
{
    public sealed class UriHelperTests
    {
        [Theory]
        [InlineData("","")]
        [InlineData("Nouveaux thèmes Visual Studio light & dark pour Prism JS", "nouveaux-themes-visual-studio-light-dark-pour-prism-js")]
        [InlineData("Nouveaux th&egrave;mes Visual Studio light &amp; dark pour Prism JS", "nouveaux-themes-visual-studio-light-dark-pour-prism-js")]
        [InlineData("Résoudre l’Erreur 217 rencontrée avec Azure Pipeline et WIX", "resoudre-lerreur-217-rencontree-avec-azure-pipeline-et-wix")]
        public void ToFriendly(String text,String expected)
        {
            var actual = UriHelper.ToFriendly(text);
            Assert.Equal(expected, actual);
        }
    }
}
