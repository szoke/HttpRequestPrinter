curl -F "binary_files[]=@cat.jfif,hedgehog.jpg" -H "Content-Type: multipart/form-data" -X POST https://localhost:44388/api/noparams --output response.bin