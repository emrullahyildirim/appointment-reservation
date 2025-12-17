const express = require('express');
const cors = require('cors');
const app = express();

app.use(cors());
app.use(express.json());


app.post('/login', (req, res) => {
    const { email, password } = req.body;

    if (!email || !password) {
        return res.status(400).json({ 
            success: false, 
            message: 'Username and password are required' 
        });
    }

	if (email === "admin@admin.com" && password === "123456")
			return res.status(200).json({success: true, message: "Giriş yapıldı!"});
	return res.status(400).json({ success: false, message: 'Wrong credentials!'});
});

const PORT = 3001;
app.listen(PORT, '0.0.0.0', () => {
    console.log(`Auth service listening on port ${PORT}`);
});