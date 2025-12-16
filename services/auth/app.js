const express = require("express");

const app = express();

app.post("/verify", (req, res) => {
	const { Authorization } = req.headers;

	
	if (!Authorization)
		return res.status(400).json({ error: true, message: "You need to provide a token.", data: {} });
	
	return res.status(200).json({ error: false, message: "", data: { userId: 5 } });
});

app.listen(3001);
