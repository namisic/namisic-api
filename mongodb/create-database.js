db = connect('mongodb://eladmin:namisic_2025@localhost/admin');
db = db.getSiblingDB('namiSIC');
db.createUser({user: 'namiUserApp', pwd: 'Aymi.gatito_MIau', roles: [{role: 'readWrite', db: 'namiSIC'}]})
db.creation.insertOne({ 'created_at': new Date()});
