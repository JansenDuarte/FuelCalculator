FuelCalculator
===

Simple tool designed to calculate fuel consumption, record refuelings,
and help decide wich fuel has the best cost-benefit ratio.


How To
===

First Start Up
---
During first start up, the app will create a SQLite database to record all the
information from subsequent uses. DB will be located in:
>   /storage/emulated/userid/Android/data/FuelCalculator/files

You'll be asked to fill in known consumptions for gasoline and ethanol, usualy present
in the owner's manual for flex vehicles. This will help the app with it's first
calculations of the cost-benefit part of it's functioning.

If you don't know these informations, just skip this step for now. You can calculate
your own consumption values, and you can also add them later.

Refuel and Consumption
---
Within the refuel section, you can record a refueling `in litres` by filling
up the input box for volume and hitting the `Save Refuel` button. This will keep a
record of your last refuel until you are ready to place the ammount of `kilometres`
you managed to run with this amount of fuel.

Once you open the app again, and go into the refuel section, you'll see the volume
recorded.
Then, after filling the kilometres you can calculate your median fuel consumption and
record it.

Cost-Benefit Calculator
---
Going into the cost-benefit calculator, you'll see two fields. One for `alcohol` price
and one for `gasoline`. Fill them up and hit the `Calculate` button.
The app will average the amount of consumptions recorded for both fuels, account in the
price of them and show you the percentage diference in price, consumption, and show
wich fuel is best advised.
*The app will not take into consideration that alcohol might screw up your spark plugs,*
*injectors and dry/rust parts of your exhaust system. Use it as a guide, and not as a law!*


Options
---
In the options section, you can set up how many of the recorded consumptions should be taken
into consideration during the cost-benefit calculations.
Engines tend to get 'loose' with time, and lose eficiency. So if you take older avareges into
consideration, the calculations might be squewed towards the unreal territory.


Saved History
---
Going into the eye icon, you can view all of the recorded consumptions you've made.

>   They are recorded in `km/L`
>   They also record the type of fuel used, `gasoline` or `alcohol`
>   And the `date` in wich it was recorded


Future Features
===
This app is in the process of going into the Google's AppStore. Once is up and running
I'll add a link here.

Some functionalities are still in the planning stage. If you have any ideas or sugestions,
fell free to hit me up with them.


Special Thanks
===
Thank you wifey, for keeping up with my shenanigans. And thank you Manolo for helping with the shenanigans!


Cheers! Keep Creating! Love, JD
===