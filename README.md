# Payment gateway

A payment gateway prototype.

## Starting the application

Run the below commands from the **/src/** directory:  

```powershell
docker-compose build
docker-compose up
```
This will start the solution with a single instance of the API services.  
Use the following URLs:
- Checkout API service: [http://localhost:5102](http://localhost:5102)  

You can use Postman for API testing. See the **/postman/** directory.
## Test card numbers
To test a bank card payment enter arbitrary expiry date, a CVC/CVV and a cardholder name.

|Number|Result
|:---|:---|
| 4000000000009995 | Declined. Lost card. |
| 4000000000000069 | Declined. Insufficient funds. |
| 4000000000000127 | Declined. Stolen card. |
| Other numbers | Authorized.                                                                                                       |

