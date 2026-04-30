/* global use, db */
// MongoDB Playground
// Use Ctrl+Space inside a snippet or a string literal to trigger completions.

// The current database to use.
use('redump');

// Search for documents in the current collection.
db.getCollection('discs')
  .find(
    {
      // Corretto: $ne invece di $neq
      "title": { $ne: 'N/D' },
      //"libcrypt_sectors": { $eq: null },
      //"pvd_entries": [],
      "disc_id": '96925'
      //"rings": null
    },
    {
      /*
      * Projection
      * _id: 0, // exclude _id
      * fieldA: 1 // include field
      */
    }
  )
  .sort({
    /*
    * fieldA: 1 // ascending
    * fieldB: -1 // descending
    */
  });
