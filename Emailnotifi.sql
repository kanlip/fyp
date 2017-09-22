SELECT c.Email ,c.Name,a.AppDate FROM Customer c , Appointment a
WHERE c.CustomerId = a.cus_id
AND a.AppStatus='A'
AND DATEDIFF(day, a.AppDate, CURRENT_TIMESTAMP)<=1
AND DATEDIFF(MONTH,a.AppDate,CURRENT_TIMESTAMP)=0;