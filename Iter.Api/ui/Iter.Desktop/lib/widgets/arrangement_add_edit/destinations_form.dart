import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/dateTime_helper.dart';
import 'package:ui/models/destinatios_form_controllers.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/dropdown_provider.dart';

class DestinationFormPage extends StatefulWidget {
  final DestinatiosFormControllers controllers;

  const DestinationFormPage({super.key, required this.controllers});

  @override
  State<DestinationFormPage> createState() => _DestinationFormPageState();
}

class _DestinationFormPageState extends State<DestinationFormPage> {
  DropdownProvider? _dropdownProvider;

  List<DropdownModel> countries = [];
  Map<int, List<DropdownModel>> cities = {};
  List<Map<int, int>> selectedCountry = [];
  List<Map<int, int>> selectedCity = [];

  @override
  void initState() {
    super.initState();
    _dropdownProvider = context.read<DropdownProvider>();

    loadCountries();
    initCityLoad();

    if (widget.controllers.destinationControllers.isEmpty) {
      addDestination();
    }
  }

  initCityLoad() async {
    if (widget.controllers.countryControllers.length > 0) {
      for (int i = 0; i < widget.controllers.countryControllers.length; i++) {
        await loadCities(widget.controllers.countryControllers[i].text, i);
      }
    }
  }

  Future<void> loadCountries() async {
    var counties = await _dropdownProvider
        ?.get({"dropdownType": DropdownTypes.countries.index});
    counties?.result
        .insert(0, new DropdownModel(id: "", name: "Izaberite drzavu"));
    setState(() {
      countries = counties!.result;
    });
  }

  Future<void> loadCities(String countryId, int index) async {
    var cityListResponse = await _dropdownProvider?.get({
      "dropdownType": DropdownTypes.cities.index.toString(),
      "countryId": countryId
    });

    if (cityListResponse != null && cityListResponse.result != null) {
      List<DropdownModel> cityList = [
        DropdownModel(id: "", name: "Izaberite grad"),
        ...cityListResponse.result
      ];

      setState(() {
        cities[index] = cityList;
      });
      print(cities);
    } else {
      setState(() {
        cities[index] = [DropdownModel(id: "", name: "Nema dostupnih gradova")];
      });
    }
  }

  void _removeDestinationRow(int index) {
    setState(() {
      widget.controllers.idControllers.removeAt(index);
      widget.controllers.destinationControllers.removeAt(index);
      widget.controllers.countryControllers.removeAt(index);
      widget.controllers.arrivalDateTimeControllers.removeAt(index);
      widget.controllers.departureDateTimeControllers.removeAt(index);
      widget.controllers.hotelNameControllers.removeAt(index);
      widget.controllers.cityControllers.removeAt(index);
      widget.controllers.postalCodeControllers.removeAt(index);
      widget.controllers.streetControllers.removeAt(index);
      widget.controllers.houseNumberControllers.removeAt(index);
      widget.controllers.isHotelIncluded.removeAt(index);
    });

    removeAndReorder(cities, countries, index);
  }

  void removeAndReorder(
      Map<int, List<DropdownModel>> cities, List<DropdownModel> countries, int indexToRemove) {
    cities.remove(indexToRemove);

    Map<int, List<DropdownModel>> newCities = {};
    int newIndex = 0;
    cities.forEach((key, value) {
      if (key != indexToRemove) {
        newCities[newIndex] = value;
        newIndex++;
      }
    });

    cities
      ..clear()
      ..addAll(newCities);
  }

  void addDestination() {
    setState(() {
      widget.controllers.idControllers.add(TextEditingController());
      widget.controllers.destinationControllers.add(TextEditingController());
      widget.controllers.countryControllers.add(TextEditingController());
      widget.controllers.arrivalDateTimeControllers
          .add(TextEditingController());
      widget.controllers.departureDateTimeControllers
          .add(TextEditingController());
      widget.controllers.hotelNameControllers.add(TextEditingController());
      widget.controllers.cityControllers.add(TextEditingController());
      widget.controllers.postalCodeControllers.add(TextEditingController());
      widget.controllers.streetControllers.add(TextEditingController());
      widget.controllers.houseNumberControllers.add(TextEditingController());
      widget.controllers.isHotelIncluded.add(false);
    });
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Column(
            children: List.generate(
                widget.controllers.destinationControllers.length, (int index) {
          return Container(
            padding: EdgeInsets.fromLTRB(0, 10, 0, 30),
            child: Column(
              children: [
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Align(
                        alignment: Alignment.bottomLeft,
                        child: Text("${index + 1}. destinacija",
                            style:
                                TextStyle(color: Colors.amber, fontSize: 15))),
                    IconButton(
                      icon: const Icon(Icons.close),
                      onPressed: () {
                        if (widget.controllers.destinationControllers.length >
                            1) {
                          _removeDestinationRow(index);
                        }
                      },
                    )
                  ],
                ),
                Row(
                  children: [
                    Expanded(
                      flex: 2,
                      child: DropdownButtonFormField<dynamic>(
                        decoration: const InputDecoration(
                          labelText: 'Izaberite državu',
                        ),
                        value:
                            widget.controllers.countryControllers[index].text ??
                                0,
                        items: countries.map((DropdownModel item) {
                          return DropdownMenuItem<dynamic>(
                            value: item.id,
                            child: Text(item.name ?? ""),
                          );
                        }).toList(),
                        onChanged: (value) async {
                          setState(() {
                            widget.controllers.countryControllers[index].text =
                                value;
                          });
                          await loadCities(value, index);
                          widget.controllers.destinationControllers[index]
                              .text = "";
                        },
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Izaberite državu';
                          }
                          return null;
                        },
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      flex: 2,
                      child: DropdownButtonFormField<dynamic>(
                        decoration: const InputDecoration(
                          labelText: 'Izaberite destinaciju',
                        ),
                        value: widget
                            .controllers.destinationControllers[index].text,
                        items: cities[index]?.map((DropdownModel item) {
                          return DropdownMenuItem<dynamic>(
                            value: item.id,
                            child: Text(item.name ?? ""),
                          );
                        }).toList(),
                        onChanged: (value) {
                          setState(() {
                            widget.controllers.destinationControllers[index]
                                .text = value;
                          });
                        },
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Morate izabrati destinaciju';
                          }
                          return null;
                        },
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      flex: 1,
                      child: TextFormField(
                        controller: widget
                            .controllers.arrivalDateTimeControllers[index],
                        decoration: InputDecoration(
                          labelText: 'Vrijeme dolaska',
                          suffixIcon: Icon(Icons.calendar_today),
                        ),
                        readOnly: true, // Sprečava pojavu tastature
                        onTap: () => DateTimeHelper.selectDateAndTime(
                            context,
                            widget
                                .controllers.arrivalDateTimeControllers[index]),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }
                          return null;
                        },
                      ),
                    ),
                    const SizedBox(width: 60),
                    Expanded(
                      flex: 1,
                      child: TextFormField(
                        controller: widget
                            .controllers.departureDateTimeControllers[index],
                        decoration: InputDecoration(
                          labelText: 'Vrijeme odlaska',
                          suffixIcon: Icon(Icons.calendar_today),
                        ),
                        readOnly: true,
                        onTap: () => DateTimeHelper.selectDateAndTime(
                            context,
                            widget.controllers
                                .departureDateTimeControllers[index]),
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Polje je obavezno';
                          }

                          DateFormat format = new DateFormat("dd-MM-yyyy");
                          DateTime arrivalDateTime = format.parse(widget.controllers
                              .arrivalDateTimeControllers[index].text);

                          DateTime departureDateTime = format.parse(value);
                          
                            if (departureDateTime.isBefore(arrivalDateTime)) {
                              return 'Vrijeme odlaska ne može biti manje od vremena dolaska';
                            }

                          return null;
                        },
                      ),
                    )
                  ],
                ),
                const SizedBox(height: 20),
                CheckboxListTile(
                  title: Text('Uključen hotel', style: TextStyle(fontSize: 15)),
                  value: widget.controllers.isHotelIncluded[index],
                  onChanged: (bool? value) {
                    setState(() {
                      widget.controllers.isHotelIncluded[index] = value!;
                    });
                  },
                  controlAffinity: ListTileControlAffinity
                      .leading, // Pozicionira checkbox na početak
                ),
                if (widget.controllers.isHotelIncluded[index] ?? false)
                  Column(
                    children: [
                      Row(
                        children: [
                          Expanded(
                            flex: 2,
                            child: TextFormField(
                              controller: widget
                                  .controllers.hotelNameControllers[index],
                              decoration: const InputDecoration(
                                  labelText: 'Naziv hotela'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 30) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                          const SizedBox(width: 60),
                          Expanded(
                            flex: 2,
                            child: TextFormField(
                              controller:
                                  widget.controllers.streetControllers[index],
                              decoration:
                                  const InputDecoration(labelText: 'Ulica'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 30) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                          const SizedBox(width: 60),
                          Expanded(
                            flex: 1,
                            child: TextFormField(
                              controller: widget
                                  .controllers.postalCodeControllers[index],
                              decoration: const InputDecoration(
                                  labelText: 'Poštanski broj'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 10) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                          const SizedBox(width: 60),
                          Expanded(
                            flex: 1,
                            child: TextFormField(
                              controller: widget
                                  .controllers.houseNumberControllers[index],
                              decoration: const InputDecoration(
                                  labelText: 'Kućni broj'),
                              validator: (value) {
                                if (value == null || value.isEmpty) {
                                  return 'Polje je obavezno';
                                }
                                if (value.length > 5) {
                                  return 'Neispravan unos';
                                }
                                return null;
                              },
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
              ],
            ),
          );
        })),
        Row(
          children: [
            InkWell(
              onTap: () {
                addDestination();
              },
              child: const Row(
                mainAxisSize: MainAxisSize.min,
                children: <Widget>[
                  Icon(
                    Icons.add_circle_rounded,
                    color: Colors.amber,
                    size: 30,
                  ),
                  Padding(
                    padding: EdgeInsets.only(left: 8),
                    child: Text(
                      'Dodaj destinaciju',
                      style: TextStyle(
                        color: Colors.amber,
                        fontSize: 16,
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ],
    );
  }
}
