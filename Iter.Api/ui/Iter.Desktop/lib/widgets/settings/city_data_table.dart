import 'package:flutter/material.dart';
import 'package:number_paginator/number_paginator.dart';
import 'package:provider/provider.dart';
import 'package:ui/enums/dropdown_types.dart';
import 'package:ui/helpers/scaffold_messenger_helper.dart';
import 'package:ui/modals/custom_confirmation_modal.dart';
import 'package:ui/modals/insert_city_modal.dart';
import 'package:ui/models/city.dart';
import 'package:ui/models/dropdown_model.dart';
import 'package:ui/services/city_provider.dart';
import 'package:ui/services/dropdown_provider.dart';
import 'package:ui/widgets/search_button.dart';

class CityDataTable extends StatefulWidget {
  const CityDataTable({super.key});

  @override
  State<CityDataTable> createState() => CityDataTableState();
}

class CityDataTableState extends State<CityDataTable> {
  TextEditingController searchController = TextEditingController();

  CityProvider? _cityProvider;
  DropdownProvider? _dropdownProvider;
  String? selectedCountry;
  List<City> cities = [];
  int? citiesCount;
  List<DropdownModel>? countriesDropdown;

  bool _displayLoader = true;

  int _currentPage = 1;
  int _pageSize = 10;

  @override
  void initState() {
    super.initState();
    _cityProvider = context.read<CityProvider>();
    _dropdownProvider = context.read<DropdownProvider>();
    initialLoad();
  }

  Future<void> initialLoad() async {
    setState(() {
      _displayLoader = true;
    });
    try {
      await search();

      var countriesDropdownTemp = await _dropdownProvider!
          .get({"dropdownType": DropdownTypes.countries.index});
      countriesDropdownTemp.result
          .insert(0, new DropdownModel(id: null, name: "Nije odabrano"));
      if (countriesDropdownTemp.result.isNotEmpty) {
        setState(() {
          countriesDropdown = countriesDropdownTemp.result;
        });
      }

    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = false;
      });
    }
  }

  Future<void> search() async {
    var searchCities = await _cityProvider?.get({
      "currentPage": _currentPage,
      "pageSize": _pageSize,
      "name": searchController.text,
      "countryId": selectedCountry,
    });

    if (searchCities != null) {
      setState(() {
        cities = searchCities.result;
        citiesCount = searchCities.count;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: <Widget>[
            Expanded(
              flex: 1,
              child: TextFormField(
                controller: searchController,
                decoration: const InputDecoration(
                  labelText: 'Unesite pretragu',
                ),
                onFieldSubmitted: (value) async {
                  await search();
                },
              ),
            ),
              const SizedBox(width: 20),
              Expanded(
                flex: 1,
                child: DropdownButtonFormField<dynamic>(
                  decoration: const InputDecoration(
                    labelText: 'Izaberite državu',
                  ),
                  value: selectedCountry,
                  items: countriesDropdown?.map((DropdownModel item) {
                    return DropdownMenuItem<dynamic>(
                      value: item.id,
                      child: Text(item.name ?? ""),
                    );
                  }).toList(),
                  onChanged: (value) {
                    setState(() {
                      selectedCountry = value;
                    });
                  },
                ),
              ),
            const SizedBox(width: 20),
            SearchButton(
              onSearch: search,
            ),
          ],
        ),
        SizedBox(height: 20),
        Row(
          mainAxisAlignment: MainAxisAlignment.end,
          children: [
          ElevatedButton(
                      onPressed: () {
                        showDialog(
                          context: context,
                          builder: (BuildContext context) {
                            return InsertCityModal(
                              onCompleted: () => search()
                            );
                          },
                        );
                      },
                      child: const Text(
                        "Dodaj grad",
                        style: TextStyle(color: Colors.white),
                      ),
                    ),
        ]),
        SizedBox(height: 20),
        _displayLoader
            ? const Center(
                child: Column(children: [CircularProgressIndicator()]))
            : Row(children: <Widget>[
                Expanded(
                  child: DataTable(
                    columns: const [
                      DataColumn(
                          label: Text('Id',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(
                          label: Text('Naziv',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(
                          label: Text('Država',
                              style: TextStyle(fontWeight: FontWeight.bold))),
                      DataColumn(label: Text('')),
                    ],
                    rows: cities
                        .map(
                          (city) => DataRow(
                            cells: [
                              DataCell(Text(city.id.toString())),
                              DataCell(Text(city.name)),
                              DataCell(Text(city.country)),
                              DataCell(
                                SizedBox(
                                  width: double.infinity,
                                  child: Row(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    children: <Widget>[
                                      IconButton(
                                          icon: const Icon(Icons.edit_document),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return InsertCityModal(
                                                    onCompleted: () => search(),
                                                    id: city.id.toString());
                                              },
                                            );
                                          },
                                          tooltip: 'Uredi'),
                                      IconButton(
                                          icon: const Icon(Icons.delete),
                                          onPressed: () {
                                            showDialog(
                                              context: context,
                                              builder: (BuildContext context) {
                                                return CustomConfirmationDialog(
                                                  title: "Potvrda brisanja?",
                                                  content:
                                                      "Da li ste sigurni da želite obrisati grad ${city.name}?",
                                                  onConfirm: () async {
                                                    await delete(city.id);
                                                  },
                                                );
                                              },
                                            );
                                          },
                                          tooltip: 'Obriši'),
                                    ],
                                  ),
                                ),
                              ),
                            ],
                          ),
                        )
                        .toList(),
                  ),
                )
              ]),
        Row(children: <Widget>[
          Expanded(
            child: Align(
              alignment: Alignment.center,
              child: Container(
                constraints: const BoxConstraints(maxWidth: 400),
                child: Visibility(
                  visible:
                      citiesCount != null && citiesCount! > _pageSize,
                  child: NumberPaginator(
                    initialPage: 0,
                    numberPages:
                        (((citiesCount == 0 || citiesCount == null)
                                    ? 1
                                    : citiesCount!) /
                                _pageSize)
                            .ceil(),
                    onPageChange: (int index) {
                      setState(() {
                        _currentPage = index + 1;
                      });
                      search();
                    },
                  ),
                ),
              ),
            ),
          )
        ])
      ],
    );
  }

  Future<void> delete(id) async {
    try {
      setState(() {
        _displayLoader = true;
      });
      await _cityProvider?.delete(id);
      search();
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Uspješno ste obrisali grad",
          backgroundColor: Colors.green);
    } catch (error) {
      ScaffoldMessengerHelper.showCustomSnackBar(
          context: context,
          message: "Došlo je do greške",
          backgroundColor: Colors.red);
    } finally {
      setState(() {
        _displayLoader = false;
      });
    }
  }

}

