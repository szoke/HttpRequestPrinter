curl --form 'files[]=@nice.zip;type=application/zip' --form 'files[]=@hello_there.zip;type=application/zip' -X POST https://localhost:44388/api/noparams --output response.bin 
