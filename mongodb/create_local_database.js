db = connect('mongodb://eladmin:namisic_2025@localhost/admin');
db = db.getSiblingDB('namisic');
db.creation.insertOne({ 'created_at': new Date()});
