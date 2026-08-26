Û	
UC:\Users\phatd\source\repos\CinemaBookingSystem\Cinema.Notification.Worker\Program.cs
var 
builder 
= 
Host 
. $
CreateApplicationBuilder +
(+ ,
args, 0
)0 1
;1 2
builder 
. 
Services 
. 
AddSingleton 
< "
IConnectionMultiplexer 4
>4 5
(5 6
sp6 8
=>9 ;!
ConnectionMultiplexer 
. 
Connect !
(! "
builder" )
.) *
Configuration* 7
.7 8
GetConnectionString8 K
(K L
$strL S
)S T
??U W
$strX h
)h i
)i j
;j k
builder		 
.		 
Services		 
.		 
AddHostedService		 !
<		! "
Worker		" (
>		( )
(		) *
)		* +
;		+ ,
var 
host 
=	 

builder 
. 
Build 
( 
) 
; 
host 
. 
Run 
( 	
)	 

;
 Ñ
TC:\Users\phatd\source\repos\CinemaBookingSystem\Cinema.Notification.Worker\Worker.cs
	namespace 	
Cinema
 
. 
Notification 
. 
Worker $
{ 
public 

class 
Worker 
: 
BackgroundService +
{ 
private 
readonly 
ILogger  
<  !
Worker! '
>' (
_logger) 0
;0 1
private 
readonly "
IConnectionMultiplexer /
_redis0 6
;6 7
public

 
Worker

 
(

 
ILogger

 
<

 
Worker

 $
>

$ %
logger

& ,
,

, -"
IConnectionMultiplexer

. D
redis

E J
)

J K
{ 	
_logger 
= 
logger 
; 
_redis 
= 
redis 
; 
} 	
	protected 
override 
async  
Task! %
ExecuteAsync& 2
(2 3
CancellationToken3 D
stoppingTokenE R
)R S
{ 	
_logger 
. 
LogInformation "
(" #
$str# c
)c d
;d e
var 
sub 
= 
_redis 
. 
GetSubscriber *
(* +
)+ ,
;, -
await 
sub 
. 
SubscribeAsync $
($ %
$str% :
,: ;
(< =
channel= D
,D E
messageF M
)M N
=>O Q
{ 
_logger 
. 
LogInformation &
(& '
$"' )
$str) T
{T U
messageU \
}\ ]
"] ^
)^ _
;_ `
_logger 
. 
LogInformation &
(& '
$str' g
)g h
;h i
} 
) 
; 
while 
( 
! 
stoppingToken !
.! "#
IsCancellationRequested" 9
)9 :
{ 
await 
Task 
. 
Delay  
(  !
$num! %
,% &
stoppingToken' 4
)4 5
;5 6
}   
}!! 	
}"" 
}## 