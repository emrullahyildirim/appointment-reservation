
const express = require('express');
const path = require('path');
const app = express();

app.use(express.static(path.join(__dirname, './build')));

app.use((req, res) => {
    res.sendFile(path.join(__dirname, './build', 'index.html'));
});

const PORT = 8080;
app.listen(PORT, '0.0.0.0', () => {
    console.log(`App listening on port ${PORT}`);
});