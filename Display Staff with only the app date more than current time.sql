SELECT Staff.Name, Staff.StaffId FROM Staff, Appointment
WHERE Staff.StaffId = Appointment.Staff_id
AND AppStatus='A'
AND AppDate >= Convert(datetime, CURRENT_TIMESTAMP )
GROUP BY Name , StaffId;