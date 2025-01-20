namespace App.Services.Helper
{
    public static class ModelConverter
    {
        public static TDestination ConvertTo<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var destination = new TDestination();

            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDestination).GetProperties();

            foreach (var sourceProp in sourceProperties)
            {
                // Find matching property in the destination by name and type
                var destinationProp = destinationProperties
                    .FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);

                if (destinationProp != null && destinationProp.CanWrite)
                {
                    destinationProp.SetValue(destination, sourceProp.GetValue(source));
                }
            }

            return destination;
        }

        // Overload for collections
        public static IEnumerable<TDestination> ConvertTo<TSource, TDestination>(IEnumerable<TSource> sourceCollection)
            where TDestination : new()
        {
            if (sourceCollection == null)
            {
                throw new ArgumentNullException(nameof(sourceCollection));
            }

            return sourceCollection.Select(source => ConvertTo<TSource, TDestination>(source)).ToList();
        }
    }
}
