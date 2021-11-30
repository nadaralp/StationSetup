# Requirements
1. Have AWS configured with permissions to create S3 bucket, list bucket and put objects

# How to use
1. Run the application EXE
2. Specify the folder path (full path) to copy from your machine to AWS S3
3. Specify the S3 bucket to copy to. If the bucket doesn't exist it will be created
   * You can additionally specify a folder the S3 bucket to save the items in

All the items will be copied from the directory specified recursively to AWS S3. Saving the folder conventions