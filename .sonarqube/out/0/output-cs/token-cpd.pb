Ü@
PF:\APIÂºÄÂèëÔºàRomensÈõ®‰∫∫Ôºâ\LearnWay\HongFireSimple\HongFireSimple\Program.cs
	namespace 	
HongFireSimple
 
{ 
class 	
Program
 
{ 
public 
readonly 
Logger 
	CommonLog (
=) *

LogManager+ 5
.5 6!
GetCurrentClassLogger6 K
(K L
)L M
;M N
private 
static 
readonly 
ConnectionFactory  1
rabbitMqFactory2 A
=B C
newD G
ConnectionFactoryH Y
(Y Z
)Z [
{ 	
HostName 
= 
$str (
,( )
UserName 
= 
$str 
, 
Password 
= 
$str 
,  
Port 
= 
$num 
, $
AutomaticRecoveryEnabled $
=% &
true' +
} 	
;	 

const 
string 
ExchangeName !
=" #
$str$ 9
;9 :
const!! 
string!! 
	QueueName!! 
=!!  
$str!!! 3
;!!3 4
static"" 
void"" 
Main"" 
("" 
string"" 
[""  
]""  !
args""" &
)""& '
{## 	
Console99 
.99 
	WriteLine99 
(99 
$str99 2
)992 3
;993 4
using:: 
(:: 
IConnection:: 
conn:: #
=::$ %
rabbitMqFactory::& 5
.::5 6
CreateConnection::6 F
(::F G
)::G H
)::H I
{;; 
using<< 
(<< 
IModel<< 
channel<< %
=<<& '
conn<<( ,
.<<, -
CreateModel<<- 8
(<<8 9
)<<9 :
)<<: ;
{== 
channel>> 
.>> 
ExchangeDeclare>> +
(>>+ ,
ExchangeName>>, 8
,>>8 9
$str>>: B
,>>B C
durable>>D K
:>>K L
true>>M Q
,>>Q R

autoDelete>>S ]
:>>] ^
false>>_ d
,>>d e
	arguments>>f o
:>>o p
null>>q u
)>>u v
;>>v w
channel?? 
.?? 
QueueDeclare?? (
(??( )
	QueueName??) 2
,??2 3
durable??4 ;
:??; <
true??= A
,??A B

autoDelete??C M
:??M N
false??O T
,??T U
	exclusive??V _
:??_ `
false??a f
,??f g
	arguments??h q
:??q r
null??s w
)??w x
;??x y
channel@@ 
.@@ 
	QueueBind@@ %
(@@% &
	QueueName@@& /
,@@/ 0
ExchangeName@@1 =
,@@= >

routingKey@@? I
:@@I J
	QueueName@@K T
)@@T U
;@@U V
varBB 
propsBB 
=BB 
channelBB  '
.BB' (!
CreateBasicPropertiesBB( =
(BB= >
)BB> ?
;BB? @
propsDD 
.DD 

PersistentDD $
=DD% &
trueDD' +
;DD+ ,
forEE 
(EE 
intEE 
iEE 
=EE  
$numEE! "
;EE" #
iEE$ %
<EE& '
$numEE( *
;EE* +
iEE, -
++EE- /
)EE/ 0
{FF 
stringGG 
contentGG &
=GG' (
$"GG) +
RabbitÊ∂àÊÅØ--GG+ 5
{GG5 6
iGG6 7
+GG8 9
$numGG: ;
}GG; <
"GG< =
;GG= >
varHH 
msgBodyHH #
=HH$ %
EncodingHH& .
.HH. /
UTF8HH/ 3
.HH3 4
GetBytesHH4 <
(HH< =
contentHH= D
)HHD E
;HHE F
channelII 
.II  
BasicPublishII  ,
(II, -
exchangeII- 5
:II5 6
ExchangeNameII7 C
,IIC D

routingKeyIIE O
:IIO P
	QueueNameIIQ Z
,IIZ [
basicPropertiesII\ k
:IIk l
propsIIm r
,IIr s
bodyIIt x
:IIx y
msgBody	IIz Å
)
IIÅ Ç
;
IIÇ É
ConsoleJJ 
.JJ  
	WriteLineJJ  )
(JJ) *
$"JJ* ,
***ÂèëÈÄÅÊó∂Èó¥:JJ, 4
{JJ4 5
DateTimeJJ5 =
.JJ= >
NowJJ> A
.JJA B
ToStringJJB J
(JJJ K
$strJJK `
)JJ` a
}JJa b
ÔºåJJb c
{JJc d
contentJJd k
}JJk l
ÂèëÈÄÅÂÆåÊàê!JJl q
"JJq r
)JJr s
;JJs t
}KK 
}LL 
}MM 
ConsoleNN 
.NN 
	WriteLineNN 
(NN 
$strNN 2
)NN2 3
;NN3 4
varPP 
factoryPP 
=PP 
newPP 
ConnectionFactoryPP /
(PP/ 0
)PP0 1
{PP2 3
HostNamePP4 <
=PP= >
$strPP? P
,PPP Q
UserNamePPR Z
=PP[ \
$strPP] c
,PPc d
PasswordPPe m
=PPn o
$strPPp x
,PPx y
VirtualHost	PPz Ö
=
PPÜ á
$str
PPà ã
,
PPã å
Port
PPå ê
=
PPë í
$num
PPì ó
,
PPó ò&
AutomaticRecoveryEnabled
PPò ∞
=
PP± ≤
true
PP≥ ∑
}
PP∏ π
;
PPπ ∫
usingQQ 
(QQ 
varQQ 

connectionQQ !
=QQ" #
factoryQQ$ +
.QQ+ ,
CreateConnectionQQ, <
(QQ< =
)QQ= >
)QQ> ?
usingRR 
(RR 
varRR 
channelRR 
=RR  

connectionRR! +
.RR+ ,
CreateModelRR, 7
(RR7 8
)RR8 9
)RR9 :
{SS 
channelTT 
.TT 
QueueDeclareTT $
(TT$ %
queueTT% *
:TT* +
$strTT, >
,TT> ?
durableUU% ,
:UU, -
trueUU. 2
,UU2 3
	exclusiveVV% .
:VV. /
falseVV0 5
,VV5 6

autoDeleteWW% /
:WW/ 0
falseWW1 6
,WW6 7
	argumentsXX% .
:XX. /
nullXX0 4
)XX4 5
;XX5 6
varZZ 
consumerZZ 
=ZZ 
newZZ "!
EventingBasicConsumerZZ# 8
(ZZ8 9
channelZZ9 @
)ZZ@ A
;ZZA B
consumer[[ 
.[[ 
Received[[ !
+=[[" $
([[% &
model[[& +
,[[+ ,
ea[[- /
)[[/ 0
=>[[1 3
{\\ 
var]] 
body]] 
=]] 
ea]] !
.]]! "
Body]]" &
;]]& '
var^^ 
message^^ 
=^^  !
Encoding^^" *
.^^* +
UTF8^^+ /
.^^/ 0
	GetString^^0 9
(^^9 :
body^^: >
)^^> ?
;^^? @
Console__ 
.__ 
	WriteLine__ %
(__% &
$str__& 9
,__9 :
message__; B
)__B C
;__C D
channelaa 
.aa 
BasicAckaa $
(aa$ %
eaaa% '
.aa' (
DeliveryTagaa( 3
,aa3 4
falseaa5 :
)aa: ;
;aa; <
}bb 
;bb 
channelcc 
.cc 
BasicConsumecc $
(cc$ %
queuecc% *
:cc* +
$strcc, >
,cc> ?
autoAckdd% ,
:dd, -
truedd. 2
,dd2 3
$strdd3 9
,dd9 :
consumeree% -
:ee- .
consumeree/ 7
)ee7 8
;ee8 9
Consolegg 
.gg 
	WriteLinegg !
(gg! "
$strgg" 6
)gg6 7
;gg7 8
Consolehh 
.hh 
ReadLinehh  
(hh  !
)hh! "
;hh" #
}ii 
Consolejj 
.jj 
ReadKeyjj 
(jj 
)jj 
;jj 
}mm 	
}nn 
}oo ã
`F:\APIÂºÄÂèëÔºàRomensÈõ®‰∫∫Ôºâ\LearnWay\HongFireSimple\HongFireSimple\Properties\AssemblyInfo.cs
[ 
assembly 	
:	 

AssemblyTitle 
( 
$str )
)) *
]* +
[		 
assembly		 	
:			 

AssemblyDescription		 
(		 
$str		 !
)		! "
]		" #
[

 
assembly

 	
:

	 
!
AssemblyConfiguration

  
(

  !
$str

! #
)

# $
]

$ %
[ 
assembly 	
:	 

AssemblyCompany 
( 
$str 
) 
] 
[ 
assembly 	
:	 

AssemblyProduct 
( 
$str +
)+ ,
], -
[ 
assembly 	
:	 

AssemblyCopyright 
( 
$str 0
)0 1
]1 2
[ 
assembly 	
:	 

AssemblyTrademark 
( 
$str 
)  
]  !
[ 
assembly 	
:	 

AssemblyCulture 
( 
$str 
) 
] 
[ 
assembly 	
:	 


ComVisible 
( 
false 
) 
] 
[ 
assembly 	
:	 

Guid 
( 
$str 6
)6 7
]7 8
[## 
assembly## 	
:##	 

AssemblyVersion## 
(## 
$str## $
)##$ %
]##% &
[$$ 
assembly$$ 	
:$$	 

AssemblyFileVersion$$ 
($$ 
$str$$ (
)$$( )
]$$) *