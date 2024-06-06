# MongoDB instructions

This document gives intructions to prepare the MongoDB database to storing all the condominium information.

## Settings collection

To create the `settings` collection, please execute the following script in MongoDB Shell:

```
db.settings.insertOne({name: '__seed__', value: true});
```

Then create the unique index for name property:

```
db.settings.createIndex({name:1}, {unique:true});
```
