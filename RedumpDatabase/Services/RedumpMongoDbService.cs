using MongoDB.Driver;
using RedumpDatabase.Models;

namespace RedumpDatabase.Services;

/// <summary>
/// Service for managing Redump disc data in MongoDB
/// </summary>
public class RedumpMongoDbService
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<DiscDocument> _discsCollection;

    public RedumpMongoDbService(string connectionString = "mongodb://localhost:27017", string databaseName = "redump")
    {
        _mongoClient = new MongoClient(connectionString);
        _database = _mongoClient.GetDatabase(databaseName);
        _discsCollection = _database.GetCollection<DiscDocument>("discs");
        
        // Create indexes for better query performance
        CreateIndexes();
    }

    private void CreateIndexes()
    {
        try
        {
            // Index on disc_id for fast lookups
            var discIdIndexModel = new CreateIndexModel<DiscDocument>(
                Builders<DiscDocument>.IndexKeys.Ascending(d => d.DiscId),
                new CreateIndexOptions { Unique = true }
            );

            // Index on system for filtering
            var systemIndexModel = new CreateIndexModel<DiscDocument>(
                Builders<DiscDocument>.IndexKeys.Ascending(d => d.System)
            );

            // Index on region for filtering
            var regionIndexModel = new CreateIndexModel<DiscDocument>(
                Builders<DiscDocument>.IndexKeys.Ascending(d => d.Region)
            );

            // Index on title for text search
            var titleIndexModel = new CreateIndexModel<DiscDocument>(
                Builders<DiscDocument>.IndexKeys.Text(d => d.Title)
            );

            // Index on serial for search
            var serialIndexModel = new CreateIndexModel<DiscDocument>(
                Builders<DiscDocument>.IndexKeys.Ascending(d => d.Serial)
            );

            _discsCollection.Indexes.CreateMany(new[] { discIdIndexModel, systemIndexModel, regionIndexModel, titleIndexModel, serialIndexModel });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating indexes: {ex.Message}");
        }
    }

    /// <summary>
    /// Insert or update a disc document
    /// </summary>
    public async Task<string> UpsertDiscAsync(DiscDocument disc)
    {
        disc.UpdatedAt = DateTime.UtcNow;
        
        var filter = Builders<DiscDocument>.Filter.Eq(d => d.DiscId, disc.DiscId);
        
        // Try to find existing document to preserve its _id
        var existingDisc = await _discsCollection.Find(filter).FirstOrDefaultAsync();
        
        if (existingDisc != null)
        {
            // Preserve existing _id
            disc.Id = existingDisc.Id;
            disc.CreatedAt = existingDisc.CreatedAt;
        }
        else
        {
            // Set creation timestamp for new documents
            disc.CreatedAt = DateTime.UtcNow;
        }
        
        var options = new ReplaceOptions { IsUpsert = true };
        var result = await _discsCollection.ReplaceOneAsync(filter, disc, options);
        return disc.DiscId;
    }

    /// <summary>
    /// Get a disc by ID
    /// </summary>
    public async Task<DiscDocument?> GetDiscByIdAsync(string discId)
    {
        var filter = Builders<DiscDocument>.Filter.Eq(d => d.DiscId, discId);
        return await _discsCollection.Find(filter).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get all discs for a specific system
    /// </summary>
    public async Task<List<DiscDocument>> GetDiscsBySystemAsync(string system)
    {
        var filter = Builders<DiscDocument>.Filter.Eq(d => d.System, system);
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Get all discs for a specific region
    /// </summary>
    public async Task<List<DiscDocument>> GetDiscsByRegionAsync(string region)
    {
        var filter = Builders<DiscDocument>.Filter.Eq(d => d.Region, region);
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Search discs by title using text search
    /// </summary>
    public async Task<List<DiscDocument>> SearchDiscsByTitleAsync(string title)
    {
        var filter = Builders<DiscDocument>.Filter.Text(title);
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Search discs by serial number
    /// </summary>
    public async Task<List<DiscDocument>> SearchBySerialAsync(string serial)
    {
        var filter = Builders<DiscDocument>.Filter.Regex(d => d.Serial, new MongoDB.Bson.BsonRegularExpression(serial, "i"));
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Search discs by CRC32 hash in tracks
    /// </summary>
    public async Task<List<DiscDocument>> SearchByCrc32Async(string crc32)
    {
        var filter = Builders<DiscDocument>.Filter.ElemMatch(d => d.Tracks, 
            Builders<TrackDocument>.Filter.Regex(t => t.Crc32, new MongoDB.Bson.BsonRegularExpression(crc32, "i")));
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Search discs by MD5 hash in tracks
    /// </summary>
    public async Task<List<DiscDocument>> SearchByMd5Async(string md5)
    {
        var filter = Builders<DiscDocument>.Filter.ElemMatch(d => d.Tracks, 
            Builders<TrackDocument>.Filter.Regex(t => t.Md5, new MongoDB.Bson.BsonRegularExpression(md5, "i")));
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Search discs by SHA1 hash in tracks
    /// </summary>
    public async Task<List<DiscDocument>> SearchBySha1Async(string sha1)
    {
        var filter = Builders<DiscDocument>.Filter.ElemMatch(d => d.Tracks, 
            Builders<TrackDocument>.Filter.Regex(t => t.Sha1, new MongoDB.Bson.BsonRegularExpression(sha1, "i")));
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Get all discs with LibCrypt protection
    /// </summary>
    public async Task<List<DiscDocument>> GetDiscsWithLibCryptAsync()
    {
        var filter = Builders<DiscDocument>.Filter.Eq(d => d.LibCrypt, "Yes");
        return await _discsCollection.Find(filter).ToListAsync();
    }

    /// <summary>
    /// Get total disc count
    /// </summary>
    public async Task<long> GetDiscCountAsync()
    {
        return await _discsCollection.CountDocumentsAsync(FilterDefinition<DiscDocument>.Empty);
    }

    /// <summary>
    /// Delete a disc by ID
    /// </summary>
    public async Task<bool> DeleteDiscAsync(string discId)
    {
        var filter = Builders<DiscDocument>.Filter.Eq(d => d.DiscId, discId);
        var result = await _discsCollection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    /// <summary>
    /// Get discs with pagination
    /// </summary>
    public async Task<List<DiscDocument>> GetDiscsAsync(int page = 1, int pageSize = 20)
    {
        var skip = (page - 1) * pageSize;
        return await _discsCollection.Find(FilterDefinition<DiscDocument>.Empty)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Get discs by multiple filters including title, serial, and hash searches
    /// </summary>
    public async Task<List<DiscDocument>> GetDiscsByMultipleFiltersAsync(
        string? title = null,
        string? serial = null,
        string? crc32 = null,
        string? md5 = null,
        string? sha1 = null,
        string? system = null, 
        string? region = null, 
        bool? hasLibCrypt = null)
    {
        var filters = new List<FilterDefinition<DiscDocument>>();

        if (!string.IsNullOrEmpty(title))
            filters.Add(Builders<DiscDocument>.Filter.Text(title));

        if (!string.IsNullOrEmpty(serial))
            filters.Add(Builders<DiscDocument>.Filter.Regex(d => d.Serial, new MongoDB.Bson.BsonRegularExpression(serial, "i")));

        if (!string.IsNullOrEmpty(crc32))
            filters.Add(Builders<DiscDocument>.Filter.ElemMatch(d => d.Tracks, 
                Builders<TrackDocument>.Filter.Regex(t => t.Crc32, new MongoDB.Bson.BsonRegularExpression(crc32, "i"))));

        if (!string.IsNullOrEmpty(md5))
            filters.Add(Builders<DiscDocument>.Filter.ElemMatch(d => d.Tracks, 
                Builders<TrackDocument>.Filter.Regex(t => t.Md5, new MongoDB.Bson.BsonRegularExpression(md5, "i"))));

        if (!string.IsNullOrEmpty(sha1))
            filters.Add(Builders<DiscDocument>.Filter.ElemMatch(d => d.Tracks, 
                Builders<TrackDocument>.Filter.Regex(t => t.Sha1, new MongoDB.Bson.BsonRegularExpression(sha1, "i"))));

        if (!string.IsNullOrEmpty(system))
            filters.Add(Builders<DiscDocument>.Filter.Eq(d => d.System, system));

        if (!string.IsNullOrEmpty(region))
            filters.Add(Builders<DiscDocument>.Filter.Eq(d => d.Region, region));

        if (hasLibCrypt.HasValue)
            filters.Add(Builders<DiscDocument>.Filter.Eq(d => d.LibCrypt, hasLibCrypt.Value ? "Yes" : "No"));

        var filter = filters.Count > 0 
            ? Builders<DiscDocument>.Filter.And(filters)
            : FilterDefinition<DiscDocument>.Empty;

        return await _discsCollection.Find(filter).ToListAsync();
    }
}
