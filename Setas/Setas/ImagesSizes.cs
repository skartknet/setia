using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Setas.Images
{
    static class ImagesSizesDictionary
    {

        private static Dictionary<ImagesSizes, ImageDimensions> items => new Dictionary<ImagesSizes, ImageDimensions>();

        static ImagesSizesDictionary()
        {
            items.Add(ImagesSizes.ListItem, new ImageDimensions(75, 75));
            items.Add(ImagesSizes.DetailPage, new ImageDimensions(125, 200));
            items.Add(ImagesSizes.MainResult, new ImageDimensions(95, 95));
            items.Add(ImagesSizes.SecondaryResult, new ImageDimensions(75, 5));
        }

        public static ImageDimensions GetValue(ImagesSizes size)
        {
            return items[size];
        }
    }

    public enum ImagesSizes
    {
        ListItem,
        DetailPage,
        MainResult,
        SecondaryResult
    }

    public struct ImageDimensions
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ImageDimensions(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
