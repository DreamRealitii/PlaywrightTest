using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class MyTest : PageTest {
    [Test]
    public async Task GoogleHomepageTest() {
        // Visit Google's website.
        await Page.GotoAsync("https://www.google.com");

        // Expect "Google Search" and "I'm Feeling Lucky" buttons to appear.
        var searchButton = Page.GetByRole(AriaRole.Button, new() { Name = "Google Search" });
        await Expect(searchButton).ToBeVisibleAsync();
        var luckyButton = Page.GetByRole(AriaRole.Button, new() { Name = "I'm Feeling Lucky" });
        await Expect(luckyButton).ToBeVisibleAsync();

        // Search for "exotic butters".
        var searchBox = Page.GetByTitle("Search");
        await searchBox.FillAsync("exotic butters");
        await searchButton.ClickAsync(new() { Force = true });
        await Page.WaitForLoadStateAsync(LoadState.Load);

        // Google throws bot check so can't do anything past here.
    }

    [Test]
    public async Task ExoticButtersTest() {
        // Visit FNAF wiki's page, fails half of the time here for no reason.
        await Page.GotoAsync("https://freddy-fazbears-pizza.fandom.com/wiki/Five_Nights_at_Freddy%27s_Wiki");

        // Navigate to Exotic Butters page using search box.
        var searchBox = Page.GetByRole(AriaRole.Navigation, new() { Name = "Fandom top navigation" }).GetByPlaceholder("Search");
        await Expect(searchBox).ToBeVisibleAsync();
        await searchBox.FillAsync("exotic butters");
        var searchResult = Page.GetByRole(AriaRole.Link, new() { Name = "Exotic Butters", Exact = true });
        await Expect(searchResult).ToBeVisibleAsync();
        await searchResult.ClickAsync();

        // Navigate to image gallery.
        var galleryLink = Page.GetByRole(AriaRole.Link, new() { Name = "Gallery" });
        await Expect(galleryLink).ToBeVisibleAsync();
        await galleryLink.ClickAsync();

        // Click on the first image that appears.
        var image = Page.GetByRole(AriaRole.Link, new() { Name = "The Exotic Butters as seen on" });
        await Expect(image).ToBeVisibleAsync();
        await image.ClickAsync();
        var imageLink = Page.GetByRole(AriaRole.Link, new() { Name = "See full size image" });
        await Expect(imageLink).ToBeVisibleAsync();
        await imageLink.ClickAsync();
    }
}